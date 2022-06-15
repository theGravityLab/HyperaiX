namespace HyperaiX.Abstractions.Messages.ConcreteModels;

public sealed record Quote : MessageElement
{
    public Quote(string messageId)
    {
        MessageId = messageId;
    }

    public string MessageId { get; init; }
}