using HyperaiX.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HyperaiX;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHyperaiX(this IServiceCollection services,
        Action<HyperaiHostedServiceOptions>? configure = null)
    {
        services.AddSingleton<IHostedService, HyperaiHostedService>();
        if (configure is not null) services.Configure(configure);
        return services;
    }
}