using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Abstractions.Events;

public record MessageEventArgs(IChat Chat, IUser User, MessageEntity Message) : GenericEventArgs;