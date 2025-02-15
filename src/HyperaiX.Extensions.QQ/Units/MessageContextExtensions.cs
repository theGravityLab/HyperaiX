using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Abstractions.Units;
using HyperaiX.Extensions.QQ.Actions;
using HyperaiX.Extensions.QQ.Messages;
using HyperaiX.Extensions.QQ.Roles;

namespace HyperaiX.Extensions.QQ.Units;

public static class MessageContextExtensions
{
    public static async Task ReplyAsync(this MessageContext self, MessageEntity message,
        CancellationToken token = default)
    {
        var attachments = message.Attachments.ToDictionary();
        attachments[MessageEntityExtensions.ATTACHMENT_REFERENCE] =
            message.Attachments[MessageEntityExtensions.ATTACHMENT_ID];
        await self.SendAsync(message with
        {
            Attachments = attachments
        }, token);
    }

    public static async Task ChangeMemberNickNameAsync(this MessageContext self, Member member, string nickname,
        CancellationToken token = default)
    {
        await self.WriteAsync(new ChangeMemberNicknameActionArgs(member, nickname), token);
    }
}