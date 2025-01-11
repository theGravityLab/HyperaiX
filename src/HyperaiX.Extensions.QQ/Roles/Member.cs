using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Extensions.QQ.Roles;

public record Member(
    ulong Id,
    string Name,
    string? Remark,
    string? NickName,
    string? Title,
    uint Level,
    DateTimeOffset MuteExpiredAt,
    Group Group) : IUser
{
    public string DisplayName => Remark ?? NickName ?? Name;
}