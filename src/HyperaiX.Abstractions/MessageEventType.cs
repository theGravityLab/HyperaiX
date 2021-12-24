using System;

namespace HyperaiX.Abstractions;

[Flags]
public enum MessageEventType
{
    None = 0x0000,
    Friend = 0x0001,
    Group = 0x0010
}