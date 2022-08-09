using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events;

public class GroupRecallEventArgs : GenericEventArgs
{
    public string MessageId { get; set; }
    public Group Group { get; set; }
    public Member Operator { get; set; }
}