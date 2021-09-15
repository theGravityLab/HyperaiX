using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HyperaiX.Abstractions.Messages
{
    public class MessageChain: IEnumerable<MessageElement>
    {
        internal IEnumerable<MessageElement> InnerElements { get; }

        private MessageChain(){}

        public MessageChain(IEnumerable<MessageElement> elements) => InnerElements = elements;
        public IEnumerator<MessageElement> GetEnumerator() => InnerElements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object? obj) => obj switch
        {
            IEnumerable<MessageElement> elements => InnerElements.SequenceEqual(elements),
            _ => false
        };

        public override int GetHashCode() => InnerElements.GetHashCode();
    }
}