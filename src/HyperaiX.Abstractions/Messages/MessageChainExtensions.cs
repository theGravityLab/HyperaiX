using System.Linq;
using System.Text.RegularExpressions;
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

        private static Regex elementRegex = new(@"\{(?<index>[0-9]+)(:(?<type>[a-zA-Z0-9]+))?\}");
        public static MessageChain Extract(this MessageChain chain, string pattern)
        {
            var elements = chain.ToArray();
            var matches = elementRegex.Matches(pattern);
            if (matches.Count > 0)
            {
                var builder = new MessageChainBuilder();
                var addedCount = 0;
                foreach (Match match in matches)
                {
                    if (match.Index > addedCount)
                    {
                        builder.AddPlain(pattern[addedCount .. match.Index]);
                    }
                    var index = int.Parse(match.Groups["index"].Value);
                    builder.Add(elements[index]);
                    addedCount = match.Index + match.Length;
                }

                if (pattern.Length > addedCount)
                {
                    builder.AddPlain(pattern[addedCount ..]);
                }

                return builder.Build();
            }
            else
            {
                return MessageChain.Construct(new Plain(pattern));
            }
        }
    }
}