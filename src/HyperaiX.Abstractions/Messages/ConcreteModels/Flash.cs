using System;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Flash : MessageElement
    {
        public Flash(Uri source)
        {
            Source = source;
        }

        public Uri Source { get; init; }
    }
}