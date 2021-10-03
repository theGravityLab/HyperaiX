
using System;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Flash: MessageElement
    {
        public Uri Source { get; init; }
        public Flash(Uri source) => Source = source;
    }
}