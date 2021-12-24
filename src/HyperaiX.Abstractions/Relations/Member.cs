using System;

namespace HyperaiX.Abstractions.Relations;

public enum GroupRole
{
    Member,
    Administrator,
    Owner
}

public record Member : User
{
    public string DisplayName { get; init; }
    public GroupRole Role { get; init; } = GroupRole.Member;
    public string Title { get; init; }
    public bool IsMuted { get; init; }
    public long GroupIdentity { get; init; }
    public Lazy<Group> Group { get; init; }

    public override string Identifier => $"{Identity}@{GroupIdentity}";
}