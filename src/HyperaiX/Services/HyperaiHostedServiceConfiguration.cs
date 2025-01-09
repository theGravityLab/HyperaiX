using HyperaiX.Middlewares;
using HyperaiX.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX.Services;

public class HyperaiHostedServiceConfiguration(IServiceCollection services, IConfiguration configuration)
{
    private readonly List<Type> _middlewares = [];
    private readonly List<ModuleBuilder> _modules = [];
    public IReadOnlyList<Type> Middlewares => _middlewares;
    public IReadOnlyList<ModuleBuilder> Modules => _modules;
    internal IServiceCollection Services => services;
    internal IConfiguration Configuration => configuration;

    public HyperaiHostedServiceConfiguration Use<T>()
        where T : MiddlewareBase
    {
        _middlewares.Add(typeof(T));
        return this;
    }

    public HyperaiHostedServiceConfiguration Mount(Action<ModuleBuilder> builder)
    {
        var module = new ModuleBuilder();
        builder(module);
        _modules.Add(module);
        return this;
    }
}