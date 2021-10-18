using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Receipts
{
    public class QueryMemberReceipt: GenericReceipt
    {
        public Group Group { get; set; }
        public Member Member { get; set; }
    }
}