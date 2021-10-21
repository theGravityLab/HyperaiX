using System;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Units.Patterns.Commands
{
    public static class CommandBuilderExtensions
    {
        public static CommandBuilder Option(this CommandBuilder builder, string l0ng, char sh0rt = default,
            string key = default)
        {
            var option = new Option(l0ng, sh0rt, key);
            builder.AddOption(option);
            return builder;
        }

        public static CommandBuilder Group(this CommandBuilder builder, long group) =>
            builder.WithCondition(new Signature($"{group}:*"));

        public static CommandBuilder Group(this CommandBuilder builder) => 
            builder.WithCondition(new Signature("*:*"));

        public static CommandBuilder Friend(this CommandBuilder builder, long friend) =>
            builder.WithCondition(new Signature($"_:{friend}"));

        public static CommandBuilder Friend(this CommandBuilder builder) =>
            builder.WithCondition(new Signature("_:*"));

        public static CommandBuilder Any(this CommandBuilder builder) =>
            throw new NotSupportedException("Signature not support ANY expression yet");
    }
}