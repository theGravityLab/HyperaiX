using System;

namespace HyperaiX.Units.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PersistenceAttribute : Attribute
{
    public PersistenceAttribute(SharingScope scope)
    {
        Scope = scope;
    }

    public SharingScope Scope { get; init; }
}