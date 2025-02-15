using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Modules;
using HyperaiX.Middlewares;
using HyperaiX.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX.Services;

public static class HyperaiHostedServiceConfigurationExtensions
{
    public static HyperaiHostedServiceConfiguration Mount<T>(this HyperaiHostedServiceConfiguration self)
        where T : ModuleBase, new()
    {
        var module = new T();
        var section = self.Configuration.GetSection("HyperaiX:Modules").GetSection(module.Key);
        module.ConfigureServices(self.Services, section);
        self.Mount(builder =>
        {
            builder.WithKey(module.Key);

            var features = new ModuleFeatureListBuilder(module.GetType());
            module.ConfigureFeatures(features);
            builder.WithFeatures(features.Build());
        });
        return self;
    }

    public static HyperaiHostedServiceConfiguration UseErrorLogging(this HyperaiHostedServiceConfiguration self)
    {
        self.Use<ErrorLoggingMiddleware>();
        return self;
    }

    public static HyperaiHostedServiceConfiguration UseBlacklist(this HyperaiHostedServiceConfiguration self)
    {
        self.Use<BlacklistMiddleware>();
        return self;
    }

    public static HyperaiHostedServiceConfiguration UseBots(this HyperaiHostedServiceConfiguration self)
    {
        self.Use<BotMiddleware>();
        return self;
    }

    public static HyperaiHostedServiceConfiguration UseUnits(this HyperaiHostedServiceConfiguration self,
        Action<UnitMiddlewareOptions>? configure = null)
    {
        self.Use<UnitMiddleware>();
        if (configure != null) self.Services.Configure(configure);
        return self;
    }
}