using System;
using System.Collections.Generic;
using HyperaiX.Abstractions.Messages;

namespace HyperaiX.Units.Attributes;

public abstract class ActionFieldAttributeBase : Attribute
{
    public abstract bool Match(MessageContext context, out IReadOnlyDictionary<string, MessageChain> properties);
}