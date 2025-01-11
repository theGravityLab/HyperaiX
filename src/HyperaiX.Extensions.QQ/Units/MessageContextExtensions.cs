using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Units;
using HyperaiX.Extensions.QQ.Messages;

namespace HyperaiX.Extensions.QQ.Units;

public static class MessageContextExtensions
{
    public static Task ReplyAsync(this MessageContext context, MessageEntity message)
    {
        var attachments = message.Attachments.ToDictionary();
        attachments[MessageEntityExtensions.ATTACHMENT_REFERENCE] =
            message.Attachments[MessageEntityExtensions.ATTACHMENT_ID];
        context.Client.WriteAsync(new SendMessageActionArgs(context.Chat, message with
        {
            Attachments = attachments
        }));
        return Task.CompletedTask;
    }
}