using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Extensions.QQ.Roles;

public record Group(ulong Id, string Name, IList<Member> Members) : IChat;