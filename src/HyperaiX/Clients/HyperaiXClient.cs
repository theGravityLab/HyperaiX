using System;
using System.Threading;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace HyperaiX.Clients
{
    public class HyperaiXClient: IHostedService
    {
        private readonly HyperaiXConfiguration _configuration;
        private readonly IServiceProvider _provider;
        private readonly IApiClient _client;

        private readonly CancellationTokenSource tokenSource = new();

        public HyperaiXClient(IServiceProvider provider, HyperaiXConfiguration configuration, IApiClient client)
        {
            _configuration = configuration;
            _provider = provider;
            _client = client;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var longRunning = new Thread(() => Pull(tokenSource.Token));
            longRunning.Start();
            return Task.CompletedTask;
        }

        private void Pull(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var evt = _client.Read();
                _configuration.Pipeline(evt, _provider);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            tokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}