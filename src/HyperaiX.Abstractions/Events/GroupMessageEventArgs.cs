using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class GroupMessageEventArgs : GenericEventArgs
    {
        public Group Group { get; set; }
        public Member Sender { get; set; }
        public MessageChain Message { get; set; }
    }
}