using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX.Abstractions.Modules;

public abstract class ModuleBase
{
    protected ModuleBase()
    {
        Key = GetType().FullName ?? string.Empty;
    }

    public virtual string Key { get; }

    public abstract void ConfigureServices(IServiceCollection services, IConfiguration configuration);

    public virtual void ConfigureFeatures(IFeatureListBuilder builder)
    {
        builder
            .WithBots()
            .WithUnits();
    }
}