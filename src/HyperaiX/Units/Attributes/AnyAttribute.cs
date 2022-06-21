using System.Collections.Generic;
using HyperaiX.Abstractions.Messages;

namespace HyperaiX.Units.Attributes;

public class AnyAttribute : ActionFieldAttributeBase
{
    public override bool Match(MessageContext context, out IReadOnlyDictionary<string, MessageChain> properties)
    {
        properties = new Dictionary<string, MessageChain>();
        return true;
    }
}