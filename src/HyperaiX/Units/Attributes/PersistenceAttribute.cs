using System;

namespace HyperaiX.Units.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PersistenceAttribute : Attribute
{
    public SharingScope Scope { get; init; }

    public PersistenceAttribute(SharingScope scope) => Scope = scope;
}