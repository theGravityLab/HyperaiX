using System;
using System.Collections.Generic;
using IBuilder;

namespace HyperaiX.Units;

public class UnitServiceConfigurationBuilder : IBuilder<UnitServiceConfiguration>
{
    private readonly List<Type> units = new();

    public UnitServiceConfiguration Build()
    {
        return new()
        {
            Units = units
        };
    }

    public UnitServiceConfigurationBuilder AddUnit(Type type)
    {
        if (!type.IsAssignableTo(typeof(UnitBase)))
            throw new ArgumentException($"{type.Name} is not sub class of UnitBase", nameof(type));

        units.Add(type);

        return this;
    }
}