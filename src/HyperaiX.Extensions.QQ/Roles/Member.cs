using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Extensions.QQ.Roles;

public record Member(ulong Id, string? Remark, string? NickName, Group Group) : IUser
{
    public string DisplayName => Remark ?? NickName ?? string.Empty;
}