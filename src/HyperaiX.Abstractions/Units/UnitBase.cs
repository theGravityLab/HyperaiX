using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Abstractions.Units;

public abstract class UnitBase
{
    public MessageContext Context { get; set; } = null!;
}