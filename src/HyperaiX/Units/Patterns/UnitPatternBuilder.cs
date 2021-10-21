using System;
using System.Collections.Generic;
using HyperaiX.Units.Patterns.Commands;
using IBuilder;

namespace HyperaiX.Units.Patterns
{
    public class UnitPatternBuilder : IBuilder<UnitPattern>
    {
        internal readonly UnitBase _host;
        private readonly List<Command> commands = new();

        internal UnitPatternBuilder(UnitBase host)
        {
            _host = host;
        }

        public UnitPattern Build()
        {
            throw new NotImplementedException();
        }

        public UnitPatternBuilder AddCommand(Command command)
        {
            commands.Add(command);
            return this;
        }
    }
}