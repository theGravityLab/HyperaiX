using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class GroupRecallEventArgs: RecallEventArgs
    {
        public Group Group { get; set; }
        public Member Operator { get; set; }
    }
}