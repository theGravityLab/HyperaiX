using System;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions.Actions;

public class QuerySelfActionArgs: GenericActionArgs
{
    public override Type Output => typeof(QuerySelfReceipt);
}