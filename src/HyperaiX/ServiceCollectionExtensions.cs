using HyperaiX.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HyperaiX;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHyperaiX(this IServiceCollection services)
    {
        services.AddSingleton<IHostedService, HyperaiHostedService>();
        return services;
    }
}