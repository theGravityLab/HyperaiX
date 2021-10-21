using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Units
{
    public record MessageContext(MessageChain Message, User Sender, Group Group, IApiClient Client);
}