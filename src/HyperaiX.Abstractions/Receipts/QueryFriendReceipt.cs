using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Receipts;

public class QueryFriendReceipt : GenericReceipt
{
    public Friend Friend { get; set; }
}