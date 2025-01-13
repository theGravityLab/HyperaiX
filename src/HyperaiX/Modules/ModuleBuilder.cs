using IBuilder;

namespace HyperaiX.Modules;

public class ModuleBuilder : IBuilder<Module>
{
    private Dictionary<Type, object> _features = new();
    private string _key = string.Empty;

    public Module Build()
    {
        return new Module(_key, _features);
    }

    public ModuleBuilder WithKey(string key)
    {
        _key = key;
        return this;
    }

    public ModuleBuilder AddFeature<T>(T feature) where T : notnull
    {
        _features.Add(typeof(T), feature);
        return this;
    }

    public ModuleBuilder WithFeatures(IReadOnlyDictionary<Type, object> features)
    {
        _features = features.ToDictionary();
        return this;
    }
}