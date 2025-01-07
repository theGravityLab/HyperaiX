using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Abstractions.Actions;

public record SendMessageActionArgs(IChat Chat, MessageEntity Message) : GenericActionArgs
{
}