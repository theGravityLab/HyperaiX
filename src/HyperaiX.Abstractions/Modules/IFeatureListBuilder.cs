using IBuilder;

namespace HyperaiX.Abstractions.Modules;

public interface IFeatureListBuilder : IBuilder<IReadOnlyDictionary<Type, object>>
{
    void SetFeatureMark(string key, object value);
}