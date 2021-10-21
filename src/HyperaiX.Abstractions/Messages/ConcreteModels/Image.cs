using System;

namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Image : MessageElement
    {
        public Image(Uri source)
        {
            Source = source;
        }

        public Uri Source { get; init; }
    }
}