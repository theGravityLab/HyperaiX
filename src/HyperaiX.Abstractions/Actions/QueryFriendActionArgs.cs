using System;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions.Actions;

public class QueryFriendActionArgs : GenericActionArgs
{
    public override Type Output => typeof(QueryFriendReceipt);

    public long FriendId { get; set; }
}