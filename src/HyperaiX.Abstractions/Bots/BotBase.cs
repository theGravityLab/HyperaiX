using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Abstractions.Bots;

public abstract class BotBase
{
    public abstract Task OnEventAsync(GenericEventArgs args);

    public Task SendActionAsync(GenericActionArgs action)
    {
        return Task.CompletedTask;
    }
}