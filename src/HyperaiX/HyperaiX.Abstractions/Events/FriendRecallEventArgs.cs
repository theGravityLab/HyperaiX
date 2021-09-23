using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class FriendRecallEventArgs: RecallEventArgs
    {
        public Friend User { get; set; }
    }
}