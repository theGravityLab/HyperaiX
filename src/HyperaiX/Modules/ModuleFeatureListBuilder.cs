using System.Reflection;
using HyperaiX.Abstractions.Bots;
using HyperaiX.Abstractions.Modules;
using HyperaiX.Abstractions.Units;
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
                if (type.FullName is not null && module.Namespace is not null &&
                    type.FullName.StartsWith(module.Namespace))
                    bot.BotTypes.Add(type);

            rv.Add(typeof(BotFeature), bot);
        }

        if (GetMark<bool>(FeatureListBuilderExtensions.MARK_USE_UNITS))
        {
            var unit = new UnitFeature();
            foreach (var type in module.Assembly.GetExportedTypes().Where(x =>
                         x is { IsPublic: true, IsClass: true, IsAbstract: false } &&
                         x.IsAssignableTo(typeof(UnitBase))))
                if (type.FullName is not null && module.Namespace is not null &&
                    type.FullName.StartsWith(module.Namespace))
                {
                    var methods = type.GetMethods()
                        .Where(x => x is { IsPublic: true, IsStatic: false, IsAbstract: false });
                    foreach (var method in methods)
                    {
                        var receiver = method.GetCustomAttribute(typeof(ReceiveAttribute<>));
                        if (receiver is not null)
                        {
                            var receiverType = receiver.GetType().GetGenericArguments().First();
                            unit.Actions.Add(new UnitFeature.BoxedAction(receiverType, type, method));
                        }
                    }
                }

            rv.Add(typeof(UnitFeature), unit);
        }

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