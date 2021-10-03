using System;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Image: MessageElement
    {
        public Uri Source { get; init; }
        public Image(Uri source) => Source = source;
    }
}