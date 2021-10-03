using System;
using System.Collections;
using System.Collections.Generic;

namespace HyperaiX.Abstractions.Relations
{
    public record Group: RelationModel
    {
        public string Name { get; init; }
        public Lazy<Member> Owner { get; init; }
        public Lazy<IEnumerable<Member>> Members { get; init; }
    }
}