using System;

namespace HyperaiX.Abstractions.Actions;

public class MemberMuteActionArgs : GenericActionArgs
{
    public long GroupId { get; set; }
    public long MemberId { get; set; }
    public TimeSpan Duration { get; set; }
}