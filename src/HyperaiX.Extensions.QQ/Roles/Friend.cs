using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Extensions.QQ.Roles;

public record Friend(ulong Id, string? Remark) : IUser
{
    public string DisplayName => Remark ?? string.Empty;
}