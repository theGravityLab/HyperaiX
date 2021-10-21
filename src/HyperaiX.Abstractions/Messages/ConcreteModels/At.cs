namespace HyperaiX.Abstractions.Messages.ConcreteModels
{
    public sealed record At : MessageElement
    {
        public At(long identity)
        {
            Identity = identity;
        }

        public long Identity { get; init; }
    }
}