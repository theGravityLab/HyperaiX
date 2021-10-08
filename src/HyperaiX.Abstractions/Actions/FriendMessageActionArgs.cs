using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Actions
{
    public class FriendMessageActionArgs: GenericActionArgs
    {
        public Friend Friend { get; set; }
        public MessageChain Message { get; set; }
    }
}