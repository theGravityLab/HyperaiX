using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Actions
{
    public class GroupMessageActionArgs: GenericActionArgs
    {
        public long GroupId { get; set; }
        public MessageChain Message { get; set; }
    }
}