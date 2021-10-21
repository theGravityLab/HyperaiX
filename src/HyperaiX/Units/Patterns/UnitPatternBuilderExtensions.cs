using System;
using HyperaiX.Units.Patterns.Commands;

namespace HyperaiX.Units.Patterns
{
    public static class UnitPatternBuilderExtensions
    {
        public static UnitPatternBuilder Command(this UnitPatternBuilder builder, string methodName,
            Action<CommandBuilder> configure)
        {
            var commandBuilder = new CommandBuilder(builder._host);
            configure?.Invoke(commandBuilder);
            commandBuilder.Bind(methodName);
            builder.AddCommand(commandBuilder.Build());
            return builder;
        }
    }
}