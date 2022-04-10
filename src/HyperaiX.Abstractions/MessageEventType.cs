using System;

namespace HyperaiX.Abstractions;

[Flags]
public enum MessageEventType
{
    None = 0b0000,
    Friend = 0b0001,
    Group = 0b0010
}