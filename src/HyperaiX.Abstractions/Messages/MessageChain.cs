using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HyperaiX.Abstractions.Messages.ConcreteModels;

namespace HyperaiX.Abstractions.Messages;

public class MessageChain : IEnumerable<MessageElement>
{
    private MessageChain()
    {
    }

    public MessageChain(IEnumerable<MessageElement> elements)
    {
        InnerElements = elements;
    }

    private IEnumerable<MessageElement> InnerElements { get; }

    public IEnumerator<MessageElement> GetEnumerator()
    {
        return InnerElements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static MessageChain Construct(params MessageElement[] chain)
    {
        return new MessageChain(chain);
    }

    public override bool Equals(object obj)
    {
        return obj switch
        {
            IEnumerable<MessageElement> elements => InnerElements.SequenceEqual(elements),
            _ => false
        };
    }

    public override int GetHashCode()
    {
        return InnerElements.GetHashCode();
    }

    public override string ToString()
    {
        return string.Join(string.Empty, InnerElements.Select(x => x switch
        {
            Plain plain => plain.Text,
            _ => x.ToString()
        }));
    }

    public string Flatten()
    {
        return string.Join(string.Empty, InnerElements.Select((x, i) => x switch
        {
            Plain plain => plain.Text,
            _ => $"{{{i}:{x.TypeName}}}"
        }));
    }
}