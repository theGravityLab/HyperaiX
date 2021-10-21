using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HyperaiX.Abstractions.Messages.ConcreteModels;

namespace HyperaiX.Abstractions.Messages
{
    public class MessageChain : IEnumerable<MessageElement>
    {
        private MessageChain()
        {
        }

        public MessageChain(IEnumerable<MessageElement> elements)
        {
            InnerElements = elements;
        }

        internal IEnumerable<MessageElement> InnerElements { get; }

        public static MessageChain Construct(params MessageElement[] chain) => new(chain);

        public IEnumerator<MessageElement> GetEnumerator()
        {
            return InnerElements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
            return string.Join("", InnerElements.Select(x => x switch
            {
                Plain plain => plain.Text,
                _ => x.ToString()
            }));
        }
    }
}