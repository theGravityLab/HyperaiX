using System;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions.Actions;

public class GroupMessageActionArgs : GenericActionArgs
{
    public override Type Output => typeof(MessageReceipt);

    public long GroupId { get; set; }
    public MessageChain Message { get; set; }
}