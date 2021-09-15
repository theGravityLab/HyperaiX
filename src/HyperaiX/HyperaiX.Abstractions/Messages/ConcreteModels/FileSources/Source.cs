namespace HyperaiX.Abstractions.Messages.ConcreteModels.FileSources
{
    public sealed record Source: MessageElement
    {
        public long MessageId { get; set; }
    }
}