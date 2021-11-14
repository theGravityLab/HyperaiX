using System;
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

        public IEnumerator<MessageElement> GetEnumerator() =>
            InnerElements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public override bool Equals(object obj) =>
            obj switch
            {
                IEnumerable<MessageElement> elements => InnerElements.SequenceEqual(elements),
                _ => false
            };

        public override int GetHashCode() =>
            InnerElements.GetHashCode();

        public override string ToString() =>
            string.Join(string.Empty, InnerElements.Select(x => x switch
            {
                Plain plain => plain.Text,
                _ => x.ToString()
            }));

        public string Flatten() =>
            string.Join(string.Empty, InnerElements.Select((x, i) => x switch
            {
                Plain plain => plain.Text,
                _ => $"{{{i}:{x.TypeName}}}"
            }));
    }
}