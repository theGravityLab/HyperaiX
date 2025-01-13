using System.Reflection;

namespace HyperaiX.Modules.Features;

public class UnitFeature
{
    public IList<BoxedAction> Actions { get; } = new List<BoxedAction>();

    public record BoxedAction(Type Receiver, Type Unit, MethodInfo Action);
}