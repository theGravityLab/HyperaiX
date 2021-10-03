using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class GroupMessageEventArgs: MessageEventArgs
    {
        public Group Group { get; set; }
        public Member Sender { get; set; }
    }
}