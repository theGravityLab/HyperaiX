using System;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Audio : MessageElement
    {
        public Audio(Uri source)
        {
            Source = source;
        }

        public Uri Source { get; init; }
    }
}