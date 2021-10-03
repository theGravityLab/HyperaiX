using System;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Video: MessageElement
    {
        public Uri Source { get; init; }
        public Video(Uri source) => Source = source;
    }
}