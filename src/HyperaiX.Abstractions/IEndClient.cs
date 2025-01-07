using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Abstractions;

public interface IEndClient
{
    void Connect();
    void Disconnect();
    
    // 主线程去 Poll Read，拉到一个事件就让线程池去 Pipeline，对 Bot.OnEventAsync 和 Unit.Action 进行异步污染
    GenericEventArgs Read();
    void Write(GenericActionArgs action);
}