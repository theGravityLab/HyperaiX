namespace HyperaiX.Abstractions.Messages.ConcreteModels;

public sealed record Source : MessageElement
{
    public Source(long id)
    {
        MessageId = id;
    }

    public long MessageId { get; init; }
}