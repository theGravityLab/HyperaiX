using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events;

public class FriendRecallEventArgs : GenericEventArgs
{
    public string MessageId { get; set; }
    public Friend User { get; set; }
}