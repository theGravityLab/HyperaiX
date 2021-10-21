namespace HyperaiX.Abstractions.Actions
{
    public class MemberCardSetActionArgs : GenericActionArgs
    {
        public long GroupId { get; set; }
        public long MemberId { get; set; }
        public string DisplayName { get; set; }
    }
}