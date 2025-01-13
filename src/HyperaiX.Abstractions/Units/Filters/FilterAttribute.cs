using System.Diagnostics.CodeAnalysis;

namespace HyperaiX.Abstractions.Units.Filters;

public abstract class FilterAttribute : Attribute
{
    public virtual bool IsMatched(MessageContext context, [MaybeNullWhen(false)] out object value)
    {
        value = null;
        return false;
    }
}