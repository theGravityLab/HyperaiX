using System;
using System.Collections.Generic;
using HyperaiX.Units;
using IBuilder;

namespace HyperaiX.Units
{
    public class UnitMiddlewareConfigurationBuilder: IBuilder<UnitMiddlewareConfiguration>
    {
        private List<Type> units = new();

        public UnitMiddlewareConfiguration Build() => new()
        {
            Units = units
        };

        public UnitMiddlewareConfigurationBuilder AddUnit(Type type)
        {
            if (!type.IsAssignableTo(typeof(UnitBase)))
            {
                throw new ArgumentException($"{type.Name} is not sub class of UnitBase", nameof(type));
            }

            units.Add(type);

            return this;
        }
    }
}