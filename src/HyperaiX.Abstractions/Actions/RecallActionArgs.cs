using HyperaiX.Abstractions.Events;

namespace HyperaiX.Abstractions.Actions
{
    public class RecallActionArgs : GenericEventArgs
    {
        public long MessageId { get; set; }
    }
}