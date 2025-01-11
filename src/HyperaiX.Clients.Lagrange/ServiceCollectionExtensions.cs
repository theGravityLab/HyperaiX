using HyperaiX.Abstractions;
using HyperaiX.Clients.Lagrange.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX.Clients.Lagrange;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLagrangeClient(this IServiceCollection services,
        Action<LagrangeClientOptions>? configure = null)
    {
        services.AddSingleton<MemoryStore>();
        services.AddSingleton<IEndClient, LagrangeClient>();
        if (configure is not null) services.Configure(configure);
        return services;
    }
}