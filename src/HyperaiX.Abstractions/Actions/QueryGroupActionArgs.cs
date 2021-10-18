using System;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions.Actions
{
    public class QueryGroupActionArgs: GenericActionArgs
    {
        public override Type Output => typeof(QueryGroupReceipt);
        
        public long GroupId { get; set; }
    }
}