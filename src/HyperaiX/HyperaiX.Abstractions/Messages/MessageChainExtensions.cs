using System.Linq;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Messages.ConcreteModels.FileSources;

namespace HyperaiX.Abstractions.Messages
{
    public static class MessageChainExtensions
    {
        public static bool CanBeReplied(this MessageChain chain) => chain.Any(x => x is Source);

        public static MessageChainBuilder MakeReply(this MessageChain chain) =>
            new MessageChainBuilder().AddQuote(((Source)chain.First(x => x is Source)).MessageId);
    }
}