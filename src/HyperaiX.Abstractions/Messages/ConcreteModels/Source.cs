namespace HyperaiX.Abstractions.Messages.ConcreteModels;

public sealed record Source : MessageElement
{
    public Source(string id)
    {
        MessageId = id;
    }

    public string MessageId { get; init; }
}