using System;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Audio: MessageElement
    {
        public Uri Source { get; init; }
        public Audio(Uri source) => Source = source;
    }
}