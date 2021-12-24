using System;

namespace HyperaiX.Abstractions.Messages.ConcreteModels;

public sealed record Video : MessageElement
{
    public Video(Uri source)
    {
        Source = source;
    }

    public Uri Source { get; init; }
}