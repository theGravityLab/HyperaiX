using System.Diagnostics.CodeAnalysis;
using Duffet.Builders;

namespace HyperaiX.Abstractions.Units.Filters;

public abstract class FilterAttribute : Attribute
{
    public virtual bool IsMatched(MessageContext context, IBankBuilder bank)
    {
        return false;
    }
}