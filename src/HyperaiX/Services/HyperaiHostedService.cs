using System.Diagnostics;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HyperaiX.Services;

public class HyperaiHostedService : IHostedService
{
    private readonly IEndClient _client;
    private readonly CancellationTokenSource _cts;
    private readonly ILogger _logger;

    private readonly MiddlewareItem _pipeline;

    public HyperaiHostedService(IServiceProvider provider, ModuleRegistry registry,
        IEndClient client,
        HyperaiHostedServiceConfiguration configuration, ILogger<HyperaiHostedService> logger)
    {
        _client = client;
        _logger = logger;

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

        _cts = new CancellationTokenSource();
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _client.ConnectAsync(cancellationToken);
        _ = Task.Run(WorkAsync, CancellationToken.None);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.DisconnectAsync(cancellationToken);
        await _cts.CancelAsync();
    }

    private async Task WorkAsync()
    {
        try
        {
            while (!_cts.IsCancellationRequested)
            {
                var evt = await _client.ReadAsync(_cts.Token);
                // 并发流水线
                // _ = Task.Run(() => _pipeline.Process(evt));
                // 串行
                _pipeline.Process(evt);
            }
        }
        catch (OperationCanceledException _)
        {
            Debug.WriteLine("HyperaiX working thread is quitting...");
        }
    }

    private class MiddlewareItem(MiddlewareBase middleware)
    {
        public MiddlewareItem? Next { get; set; }

        public void Process(GenericEventArgs args)
        {
            middleware.Process(args, () => GoNext(args));
        }

        private void GoNext(GenericEventArgs args)
        {
            Next?.Process(args);
        }
    }
}