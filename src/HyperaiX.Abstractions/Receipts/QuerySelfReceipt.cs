using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Receipts;

public class QuerySelfReceipt : GenericReceipt
{
    public Self Info { get; set; }
}