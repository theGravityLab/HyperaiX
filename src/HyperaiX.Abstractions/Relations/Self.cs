using System;
using System.Collections.Generic;

namespace HyperaiX.Abstractions.Relations;

public record Self : User
{
    public Lazy<IEnumerable<Group>> Groups { get; init; }
    public Lazy<IEnumerable<Friend>> Friends { get; init; }
}