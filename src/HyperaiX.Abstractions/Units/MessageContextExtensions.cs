using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions.Units;

public static class MessageContextExtensions
{
    public static async Task<TR?> WriteAsync<TA, TR>(this MessageContext self, TA action,
        CancellationToken token = default)
        where TA : GenericActionArgs
        where TR : GenericReceiptArgs
        => await self.Client.WriteAsync(action, token) as TR;


    public static async Task WriteAsync<TA>(this MessageContext self, TA action,
        CancellationToken token = default)
        where TA : GenericActionArgs
        => await self.WriteAsync<TA, GenericReceiptArgs>(action, token);

    public static async Task<TR?> SendAsync<TR>(this MessageContext self, MessageEntity message,
        CancellationToken token = default)
        where TR : GenericReceiptArgs
        => await self.WriteAsync<SendMessageActionArgs, TR>(new SendMessageActionArgs(self.Chat, message), token);

    public static async Task SendAsync(this MessageContext self, MessageEntity message,
        CancellationToken token = default)
        => await self.SendAsync<GenericReceiptArgs>(message, token);
}