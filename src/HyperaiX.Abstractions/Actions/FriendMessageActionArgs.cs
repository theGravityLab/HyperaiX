using System;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions.Actions;

public class FriendMessageActionArgs : GenericActionArgs
{
    public override Type Output => typeof(MessageReceipt);

    public long FriendId { get; set; }
    public MessageChain Message { get; set; }
}