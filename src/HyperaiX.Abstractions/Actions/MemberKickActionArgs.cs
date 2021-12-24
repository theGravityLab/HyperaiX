namespace HyperaiX.Abstractions.Actions;

public class MemberKickActionArgs : GenericActionArgs
{
    public long GroupId { get; set; }
    public long MemberId { get; set; }
}