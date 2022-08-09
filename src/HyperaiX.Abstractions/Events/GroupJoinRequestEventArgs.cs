using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events;

public class GroupJoinRequestEventArgs : GenericEventArgs
{
    public long RequestId { get; set; }
    public string Message { get; set; }
    public Group Group { get; set; }
    public long UserId { get; set; }
}