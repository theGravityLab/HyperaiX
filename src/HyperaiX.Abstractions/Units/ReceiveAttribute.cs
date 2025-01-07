using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Abstractions.Units;

[AttributeUsage(AttributeTargets.Method)]
public class ReceiveAttribute<T> : Attribute
    where T : IChat
{
}