using HyperaiX.Abstractions.Bots;
using HyperaiX.Abstractions.Modules;
using HyperaiX.Modules.Features;

namespace HyperaiX.Modules;

public class ModuleFeatureListBuilder(Type module) : IFeatureListBuilder
{
    private readonly Dictionary<Type, object> _list = new();
    private readonly Dictionary<string, object> _marks = new();

    public IReadOnlyDictionary<Type, object> Build()
    {
        var rv = _list.ToDictionary();
        if (GetMark<bool>(FeatureListBuilderExtensions.MARK_USE_BOTS))
        {
            var bot = new BotFeature();
            foreach (var type in module.Assembly.GetExportedTypes().Where(x =>
                         x is { IsPublic: true, IsClass: true, IsAbstract: false } &&
                         x.IsAssignableTo(typeof(BotBase))))
            {
                if (type.FullName is not null && module.Namespace is not null &&
                    type.FullName.StartsWith(module.Namespace))
                {
                    bot.BotTypes.Add(type);
                }
            }
            rv.Add(typeof(BotFeature), bot);
        }
        
        // TODO: add units

        return rv;
    }

    public void SetFeatureMark(string key, object value)
    {
        _marks[key] = value;
    }

    public ModuleFeatureListBuilder Add<T>(T feature) where T : notnull
    {
        _list[typeof(T)] = feature;
        return this;
    }

    private T? GetMark<T>(string key)
    {
        if (_marks.TryGetValue(key, out var value) && value is T mark) return mark;
        return default;
    }
}