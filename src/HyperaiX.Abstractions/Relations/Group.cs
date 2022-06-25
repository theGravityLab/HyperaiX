using System;
using System.Collections.Generic;

namespace HyperaiX.Abstractions.Relations;

public record Group : Contact
{
    public string Name { get; init; }
    public Lazy<IEnumerable<Member>> Members { get; init; }
}