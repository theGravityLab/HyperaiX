namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record Quote: MessageElement
    {
        public Quote(long messageId)
        {
            MessageId = messageId;
        }

        public long MessageId { get; init; }
    }
}