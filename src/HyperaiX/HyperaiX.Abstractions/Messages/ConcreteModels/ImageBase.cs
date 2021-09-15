namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public abstract record ImageBase: StreamFileBase
    {
        public string ImageId { get; init; }
    }
}