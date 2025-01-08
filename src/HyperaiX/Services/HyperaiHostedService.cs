using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Middlewares;
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

    public HyperaiHostedService(IServiceProvider provider,
        IEndClient client,
        IOptions<HyperaiHostedServiceOptions> options)
    {
        _client = client;

        var head = new MiddlewareItem(new DummyMiddleware());
        var current = head;

        foreach (var item in options.Value.Middlewares)
        {
            var middleware = (MiddlewareBase)ActivatorUtilities.CreateInstance(provider, item);
            current.Next = new MiddlewareItem(middleware);
            current = current.Next;
        }

        _pipeline = head;
        _thread = new Thread(Work);
        _thread.Name = "HyperaiHostedService Polling Worker";
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