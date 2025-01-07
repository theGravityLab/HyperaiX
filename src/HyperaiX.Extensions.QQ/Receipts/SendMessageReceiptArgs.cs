using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Extensions.QQ.Receipts;

public record SendMessageReceiptArgs(string MessageId) : GenericReceipt;