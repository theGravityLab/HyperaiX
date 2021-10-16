using System;
using HyperaiX.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHyperaiX(this IServiceCollection services,
            Action<HyperaiXConfigurationBuilder> configure)
        {
            var builder = new HyperaiXConfigurationBuilder();
            configure?.Invoke(builder);
            
            services
                .AddSingleton( provider => builder.Build())
                .AddScoped<HyperaiXClient>();
            
            return services;
        }

        public static IServiceCollection AddHyperaiX(this IServiceCollection services) => AddHyperaiX(services, null);
    }
}