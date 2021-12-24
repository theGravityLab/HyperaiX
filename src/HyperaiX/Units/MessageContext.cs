using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Units;

public record MessageContext(MessageChain Message, MessageEventType Type, User Sender, Group Group, IApiClient Client);