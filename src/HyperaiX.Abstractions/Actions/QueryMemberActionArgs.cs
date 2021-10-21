using System;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions.Actions
{
    public class QueryMemberActionArgs : GenericActionArgs
    {
        public override Type Output => typeof(QueryMemberReceipt);

        public long GroupId { get; set; }
        public long MemberId { get; set; }
    }
}