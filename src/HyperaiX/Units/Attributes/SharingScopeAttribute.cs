using System;

namespace HyperaiX.Units.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class SharingScopeAttribute : Attribute
{
    public SharingScopeAttribute(UnitContextScope scope)
    {
        Scope = scope;
    }

    public UnitContextScope Scope { get; set; }
}