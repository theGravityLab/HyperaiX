namespace HyperaiX.Abstractions.Modules;

public interface IFeatureListBuilder : IBuilder.IBuilder<IReadOnlyDictionary<Type, object>>
{
    void SetFeatureMark(string key, object value);
}