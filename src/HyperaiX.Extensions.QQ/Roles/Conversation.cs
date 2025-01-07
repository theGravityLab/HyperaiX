using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Extensions.QQ.Roles;

// 这个谈话者有可能是 Friend，也有可能是 Member，甚至是 Anonymous
// 分别表示与好友私聊，与群友或临时查找发起消息
// 这三个 CLR 类型代表客户端可以确定并提供充足证据和数据的关系，不代表事实关系
// 可以通过辅助函数确认一个 Member 是否是 Friend
public record Conversation(IUser Conversant) : IChat;