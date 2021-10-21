using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class GroupJoinEventArgs : GenericEventArgs
    {
        public Group Group { get; set; }
        public Member Operator { get; set; }
        public Member User { get; set; }
    }
}