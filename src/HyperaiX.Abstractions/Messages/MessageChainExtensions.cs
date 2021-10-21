using System.Linq;
using HyperaiX.Abstractions.Messages.ConcreteModels;

namespace HyperaiX.Abstractions.Messages
{
    public static class MessageChainExtensions
    {
        public static bool CanBeReplied(this MessageChain chain)
        {
            return chain.Any(x => x is Source);
        }

        public static MessageChainBuilder MakeReply(this MessageChain chain)
        {
            return new MessageChainBuilder().AddQuote(((Source)chain.First(x => x is Source)).MessageId);
        }
    }
}