using System.Diagnostics;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace HyperaiX.Services;

public class HyperaiHostedService : IHostedService
{
    private class MiddlewareItem(MiddlewareBase middleware)
    {
        public MiddlewareItem? Next { get; set; }

        public void Process(GenericEventArgs args) => middleware.Process(args, () => GoNext(args));

        private void GoNext(GenericEventArgs args) => Next?.Process(args);
    }

    private readonly IEndClient _client;

    private readonly MiddlewareItem _pipeline;
    private readonly Thread _thread;
    private readonly CancellationTokenSource _cts;

    public HyperaiHostedService(IServiceProvider provider, ModuleRegistry registry,
        IEndClient client,
        HyperaiHostedServiceConfiguration configuration)
    {
        _client = client;

        #region Build Pipeline

        var head = new MiddlewareItem(new DummyMiddleware());
        var current = head;

        foreach (var item in configuration.Middlewares)
        {
            var middleware = (MiddlewareBase)ActivatorUtilities.CreateInstance(provider, item);
            current.Next = new MiddlewareItem(middleware);
            current = current.Next;
        }

        _pipeline = head;

        #endregion

        #region Build Modules

        var disabled = configuration.Configuration.GetSection("HyperaiX:Modules:Disabled").Get<string[]>() ?? [];

        foreach (var builder in configuration.Modules)
        {
            var module = builder.Build();
            module.IsActive = !disabled.Contains(module.Key);
            registry.Add(module);
        }

        #endregion

        _thread = new Thread(Work)
        {
            Name = "HyperaiHostedService Polling Worker"
        };
        _cts = new CancellationTokenSource();
    }

    private void Work()
    {
        try
        {
            while (!_cts.IsCancellationRequested)
            {
                var evt = _client.Read(_cts.Token);
                Task.Run(() => _pipeline.Process(evt));
            }
        }
        catch (OperationCanceledException _)
        {
            Debug.WriteLine("HyperaiX working thread is quitting...");
        }
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _client.ConnectAsync(cancellationToken);
        _thread.Start();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.DisconnectAsync(cancellationToken);
        await _cts.CancelAsync();
    }
}