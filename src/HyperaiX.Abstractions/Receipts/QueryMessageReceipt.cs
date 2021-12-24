using HyperaiX.Abstractions.Messages;

namespace HyperaiX.Abstractions.Receipts;

public class QueryMessageReceipt : GenericReceipt
{
    public MessageChain Message { get; set; }
}