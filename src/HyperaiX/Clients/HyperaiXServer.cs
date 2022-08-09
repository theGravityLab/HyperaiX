using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HyperaiX.Clients;

public class HyperaiXServer : IHostedService
{
    private readonly IApiClient _client;
    private readonly HyperaiXConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IServiceProvider _provider;

    private readonly CancellationTokenSource tokenSource = new();

    public HyperaiXServer(IServiceProvider provider, HyperaiXConfiguration configuration, IApiClient client,
        ILogger<HyperaiXServer> logger)
    {
        _configuration = configuration;
        _provider = provider;
        _client = client;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var longRunning = new Thread(() => Pull(tokenSource.Token));
        longRunning.Name = "Event Pulling";
        longRunning.Start();
        _logger.LogInformation("HyperaiX Server starts running");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        tokenSource.Cancel();
        return Task.CompletedTask;
    }

    private void Pull(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var evt = _client.Read(token);
            _configuration.Pipeline(evt, scope.ServiceProvider);
        }

        _logger.LogInformation("task cancelled");
    }
}