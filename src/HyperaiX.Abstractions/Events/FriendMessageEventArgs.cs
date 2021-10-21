using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class FriendMessageEventArgs : GenericEventArgs
    {
        public Friend Sender { get; set; }
        public MessageChain Message { get; set; }
    }
}