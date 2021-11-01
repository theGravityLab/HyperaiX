using System;
using HyperaiX.Abstractions;

namespace HyperaiX.Units.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReceiverAttribute : Attribute
    {
        public MessageEventType Type { get; set; }

        public ReceiverAttribute(MessageEventType type) => Type = type;
    }
}