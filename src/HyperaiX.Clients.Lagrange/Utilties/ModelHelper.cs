using System.Diagnostics;
using System.Net.Mail;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using HyperaiX.Extensions.QQ.Messages;
using HyperaiX.Extensions.QQ.Messages.Payloads;
using HyperaiX.Extensions.QQ.Messages.Payloads.Elements;
using HyperaiX.Extensions.QQ.Roles;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Entity = Lagrange.Core.Message.Entity;

namespace HyperaiX.Clients.Lagrange.Utilties;

public static class ModelHelper
{
    public static Member ToMember(Group group, ulong id, BotGroupMember? reference)
    {
        return reference != null
            ? new Member(reference.Uin, reference.MemberName, null, reference.MemberCard, reference.SpecialTitle,
                reference.GroupLevel, reference.ShutUpTimestamp, group)
            : new Member(id, "Unknown", null, null, null, 0, DateTime.MaxValue, group);
    }

    public static Group ToGroup(ulong id, BotGroup? reference)
    {
        return reference != null
            ? new Group(reference.GroupUin, reference.GroupName, new List<Member>((int)reference.MemberCount))
            : new Group(id, "Unknown", new List<Member>());
    }

    public static Friend ToFriend(ulong id, BotFriend? reference)
    {
        return reference != null
            ? new Friend(reference.Uin, reference.Nickname, reference.Remarks)
            : new Friend(id, "Unknown", null);
    }

    public static MessageEntity ToMessage(MessageChain chain)
    {
        var attachments = new Dictionary<string, object>
        {
            [MessageEntityExtensions.ATTACHMENT_ID] = chain.MessageId,
            [MessageEntityExtensions.ATTACHMENT_SENDER] = (ulong)chain.FriendUin
        };

        if (chain.FirstOrDefault(x => x is MultiMsgEntity) is MultiMsgEntity multi)
        {
            var messages = multi.Chains.Select(ToMessage).ToList();
            var rv = new MessageEntity(multi.ToPreviewString(), new Forward(messages), attachments, chain.Time);
            return rv;
        }

        if (chain.FirstOrDefault(x => x is ForwardEntity) is ForwardEntity forward)
        {
            // TODO: 在发送的时候回复需要携带整个目标消息对象
            throw new NotImplementedException();
        }

        // TODO: 文档里写的 File 和 Video 之类模糊不清，这里日后支持

        var elements = new List<IMessageElement>();
        var content = new RichContent(elements);

        foreach (var element in chain)
        {
            switch (element)
            {
                case TextEntity text:
                    elements.Add(new Text(text.Text));
                    break;
                case ImageEntity image:
                    // 这个 image.ImageUrl 似乎出现在接收时，ImagePath 则是发送时，但他俩都是非空已初始化，不懂
                    Debug.WriteLine(image);
                    elements.Add(new Image(new Uri(image.ImageUrl)));
                    break;
                case MentionEntity mention:
                    elements.Add(new At(mention.Uin, mention.Name ?? mention.Uid));
                    break;
            }
        }

        return new MessageEntity(chain.ToPreviewString(), content, attachments, chain.Time);
    }
}