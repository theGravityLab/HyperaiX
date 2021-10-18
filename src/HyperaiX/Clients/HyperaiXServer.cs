using System;
using System.Threading;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyperaiX.Clients
{
    public class HyperaiXServer: IHostedService
    {
        private readonly HyperaiXConfiguration _configuration;
        private readonly IServiceProvider _provider;
        private readonly IApiClient _client;
        private readonly ILogger _logger;

        private readonly CancellationTokenSource tokenSource = new();

        public HyperaiXServer(IServiceProvider provider, HyperaiXConfiguration configuration, IApiClient client, ILogger<HyperaiXServer> logger)
        {
            _configuration = configuration;
            _provider = provider;
            _client = client;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var longRunning = new Thread(() => Pull(tokenSource.Token));
            longRunning.Start();
            _logger.LogInformation("HyperaiX Server starts running");
            return Task.CompletedTask;
        }

        private void Pull(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var evt = _client.Read();
                _configuration.Pipeline(evt, _provider);
            }

            _logger.LogInformation("task cancelled");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            tokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}