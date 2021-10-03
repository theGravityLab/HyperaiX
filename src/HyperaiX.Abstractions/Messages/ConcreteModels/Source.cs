namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Source: MessageElement
    {
        public long MessageId { get; init; }
        public Source(long id) => MessageId = id;
    }
}