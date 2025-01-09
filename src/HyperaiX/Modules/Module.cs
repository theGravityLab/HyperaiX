namespace HyperaiX.Modules;

public class Module(string key, IReadOnlyDictionary<Type, object> features)
{
    public string Key => key;
    public IReadOnlyDictionary<Type, object> Features => features;

    public bool IsActive { get; set; } = true;
}