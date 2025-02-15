using HyperaiX.Abstractions.Actions;
using HyperaiX.Extensions.QQ.Roles;

namespace HyperaiX.Extensions.QQ.Actions;

public record ChangeMemberNicknameActionArgs(Member Member, string Nickname) : GenericActionArgs
{
}