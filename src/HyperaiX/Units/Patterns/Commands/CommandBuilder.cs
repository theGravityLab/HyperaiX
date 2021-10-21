using System.Collections.Generic;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Relations;
using IBuilder;

namespace HyperaiX.Units.Patterns.Commands
{
    public class CommandBuilder : IBuilder<Command>
    {
        internal readonly UnitBase _host;

        private string methodName;
        private readonly List<Option> options = new();
        private string prefix = "!";
        private string text;
        private Signature condition;

        internal CommandBuilder(UnitBase host)
        {
            _host = host;
        }

        public Command Build()
        {
            return new(_host.GetType().GetMethod(methodName), condition, prefix, text, options.AsReadOnly());
        }

        public CommandBuilder Bind(string methodName)
        {
            this.methodName = methodName;
            return this;
        }

        public CommandBuilder WithPrefix(string prefix)
        {
            this.prefix = prefix;
            return this;
        }

        public CommandBuilder WithText(string text)
        {
            this.text = text;
            return this;
        }

        public CommandBuilder AddOption(Option option)
        {
            options.Add(option);
            return this;
        }

        public CommandBuilder WithCondition(Signature condition)
        {
            this.condition = condition;
            return this;
        }
    }
}