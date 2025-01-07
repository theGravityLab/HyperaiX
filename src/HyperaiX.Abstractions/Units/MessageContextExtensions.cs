using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Abstractions.Units;

public static class MessageContextExtensions
{
    public static Task SendAsync(this MessageContext context, MessageEntity message)
    {
        context.Client.Write(new SendMessageActionArgs(context.Chat, message));
        return Task.CompletedTask;
    }
}