using System;
using HyperaiX.Abstractions;

namespace HyperaiX.Units.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ReceiverAttribute : Attribute
{
    public ReceiverAttribute(MessageEventType type)
    {
        Type = type;
    }

    public MessageEventType Type { get; set; }
}