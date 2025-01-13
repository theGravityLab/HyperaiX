using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Messages;

namespace HyperaiX.Abstractions.Units;

public static class MessageContextExtensions
{
    public static Task SendAsync(this MessageContext context, MessageEntity message)
    {
        context.Client.WriteAsync(new SendMessageActionArgs(context.Chat, message));
        return Task.CompletedTask;
    }
}