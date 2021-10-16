using System;
using System.Threading;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace HyperaiX.Clients
{
    public class HyperaiXClient
    {
        private readonly HyperaiXConfiguration _configuration;
        private readonly IServiceProvider _provider;

        public HyperaiXClient(HyperaiXConfiguration configuration, IServiceProvider provider)
        {
            _configuration = configuration;
            _provider = provider;
        }

        public void Publish(GenericEventArgs args)
        {
            _configuration.Pipeline(args, _provider);
        }
    }
}