using System;
using HyperaiX.Clients;
using HyperaiX.Units;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHyperaiX(this IServiceCollection services,
        Action<HyperaiXConfigurationBuilder> configure)
    {
        var builder = new HyperaiXConfigurationBuilder();
        configure?.Invoke(builder);

        services
            .AddSingleton(provider => builder.Build())
            .AddHostedService<HyperaiXServer>();

        return services;
    }

    public static IServiceCollection AddHyperaiX(this IServiceCollection services)
    {
        return AddHyperaiX(services, null);
    }

    public static IServiceCollection AddUnits(this IServiceCollection services,
        Action<UnitServiceConfigurationBuilder> configure)
    {
        var builder = new UnitServiceConfigurationBuilder();
        configure?.Invoke(builder);
        services.AddSingleton(builder.Build());
        services.AddSingleton<UnitService>();
        return services;
    }
}