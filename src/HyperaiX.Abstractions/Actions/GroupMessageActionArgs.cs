using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Actions
{
    public class GroupMessageActionArgs: GenericActionArgs
    {
        public Group Group { get; set; }
        public MessageChain Message { get; set; }
    }
}