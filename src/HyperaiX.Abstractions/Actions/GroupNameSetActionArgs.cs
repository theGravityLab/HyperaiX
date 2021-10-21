namespace HyperaiX.Abstractions.Actions
{
    public class GroupNameSetActionArgs : GenericActionArgs
    {
        public long GroupId { get; set; }
        public string GroupName { get; set; }
    }
}