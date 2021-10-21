using System.Collections.Generic;
using System.Reflection;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Units.Patterns.Commands
{
    public record Command(MethodInfo Method, Signature Condition, string Prefix, string Text,
        IEnumerable<Option> Options);
}