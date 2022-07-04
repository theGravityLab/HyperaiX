using System.Collections.Generic;
using IBuilder;

namespace HyperaiX.Abstractions.Messages;

public class MessageChainBuilder : IBuilder<MessageChain>
{
    private readonly List<MessageElement> inner = new();

    public MessageChain Build()
    {
        return new MessageChain(inner);
    }

    public MessageChainBuilder Add(MessageElement element)
    {
        if (element != null) inner.Add(element);
        return this;
    }

    public MessageChainBuilder AddRange(IEnumerable<MessageElement> elements)
    {
        inner.AddRange(elements);
        return this;
    }
}