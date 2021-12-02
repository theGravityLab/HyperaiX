using System;
using System.Collections;
using System.Collections.Generic;
using HyperaiX.Abstractions.Messages;

namespace HyperaiX.Units.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ActionExtractorAttribute: Attribute
    {
        public abstract bool Match(MessageContext context, out IReadOnlyDictionary<string, MessageChain> properties);
    }
}