using System;
using HyperaiX.Abstractions.Messages;

namespace HyperaiX.Abstractions.Events
{
    public class MessageEventArgs: GenericEventArgs
    {
        public MessageChain Message { get; set; }
    }
}