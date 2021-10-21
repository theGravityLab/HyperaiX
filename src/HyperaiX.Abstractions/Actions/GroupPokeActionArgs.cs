namespace HyperaiX.Abstractions.Actions
{
    public class GroupPokeActionArgs : GenericActionArgs
    {
        public long GroupId { get; set; }
        public long MemberId { get; set; }
    }
}