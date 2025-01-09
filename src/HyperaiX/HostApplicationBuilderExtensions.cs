using HyperaiX.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HyperaiX;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddHyperaiX(this IHostApplicationBuilder app,
        Action<HyperaiHostedServiceConfiguration>? configure = null)
    {
        var cfg = new HyperaiHostedServiceConfiguration(app.Services, app.Configuration);
        app.Services.AddSingleton<ModuleRegistry>();
        app.Services.AddSingleton<IHostedService, HyperaiHostedService>();
        configure?.Invoke(cfg);
        app.Services.AddSingleton(cfg);
        return app;
    }
}