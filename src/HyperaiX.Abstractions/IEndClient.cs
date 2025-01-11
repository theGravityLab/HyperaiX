using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX.Abstractions;

public interface IEndClient
{
    Task ConnectAsync(CancellationToken token);
    Task DisconnectAsync(CancellationToken token);

    // 主线程去 Poll Read，拉到一个事件就让线程池去 Pipeline，对 Bot.OnEventAsync 和 Unit.Action 进行异步污染
    ValueTask<GenericEventArgs> ReadAsync(CancellationToken token);
    ValueTask<GenericReceiptArgs> WriteAsync(GenericActionArgs action, CancellationToken token = default);
}