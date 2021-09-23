namespace HyperaiX.Abstractions.Events
{
    public abstract class RecallEventArgs: GenericEventArgs
    {
        public long MessageId { get; set; }
    }
}