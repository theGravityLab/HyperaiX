using System;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions.Actions
{
    public class QueryMessageActionArgs : GenericActionArgs
    {
        public override Type Output => typeof(QueryMessageReceipt);

        public long MessageId { get; set; }
    }
}