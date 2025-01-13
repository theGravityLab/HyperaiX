using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using HyperaiX.Abstractions.Roles;
using HyperaiX.Extensions.QQ.Messages;
using HyperaiX.Extensions.QQ.Messages.Payloads;
using HyperaiX.Extensions.QQ.Messages.Payloads.Elements;
using HyperaiX.Extensions.QQ.Roles;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

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
            [MessageEntityExtensions.ATTACHMENT_ID] = chain.Sequence,
            [MessageEntityExtensions.ATTACHMENT_SENDER] = (ulong)chain.FriendUin
        };

        if (chain.FirstOrDefault(x => x is MultiMsgEntity) is MultiMsgEntity multi)
        {
            var messages = multi.Chains.Select(ToMessage).ToList();
            var rv = new MessageEntity(multi.ToPreviewString(), new Forward(messages), attachments, chain.Time);
            return rv;
        }

        // TODO: 文档里写的 File 和 Video 之类模糊不清，这里日后支持

        var elements = new List<IMessageElement>();
        var content = new RichContent(elements);

        foreach (var element in chain)
            switch (element)
            {
                case TextEntity text:
                    elements.Add(new Text(text.Text));
                    break;
                case ImageEntity image when Uri.IsWellFormedUriString(image.ImageUrl, UriKind.Absolute):
                    // 这个 image.ImageUrl 似乎出现在接收时，ImagePath 则是发送时，但他俩都是非空已初始化，不懂
                    elements.Add(new Image(new Uri(image.ImageUrl)));
                    break;
                case MentionEntity mention:
                    elements.Add(new At(mention.Uin, mention.Name ?? mention.Uid));
                    break;

                // 以下是附加信息
                case ForwardEntity forward:
                    attachments[MessageEntityExtensions.ATTACHMENT_REFERENCE] = forward.Sequence;
                    break;
            }

        return new MessageEntity(chain.ToPreviewString(), content, attachments, chain.Time);
    }

    public static MessageChain FromMessage(MessageBuilder builder, MessageEntity entity)
    {
        if (entity.Attachments.TryGetValue(MessageEntityExtensions.ATTACHMENT_REFERENCE, out var value) &&
            value is uint sequence)
            builder.Add(new ForwardEntity
            {
                Sequence = sequence
            });

        switch (entity.Body)
        {
            case RichContent rich:
            {
                foreach (var element in rich.Elements)
                    switch (element)
                    {
                        case Text text:
                        {
                            builder.Text(text.Plain);
                            break;
                        }
                        case Image image:
                        {
                            builder.Image(image.Url.AbsoluteUri);
                            break;
                        }
                        case At at:
                        {
                            builder.Mention((uint)at.MemberId, at.Display);
                            break;
                        }
                    }

                break;
            }
            case Forward forward:
            {
                var chains = new MessageChain[forward.Messages.Count];
                for (var i = 0; i < forward.Messages.Count; i++)
                    // TODO: 添加 selfId
                    chains[i] = FromMessage(0ul, forward.Messages[i]);

                builder.MultiMsg(chains);
                break;
            }
        }

        return builder.Build();
    }

    public static MessageChain FromMessage(ulong selfId, MessageEntity entity)
    {
        var builder = MessageBuilder.Friend((uint)selfId);
        return FromMessage(builder, entity);
    }

    public static MessageChain FromMessage(IChat chat, MessageEntity entity)
    {
        var builder = chat switch
        {
            Conversation { Conversant: Friend friend } => MessageBuilder.Friend(
                (uint)friend.Id),
            Group group => MessageBuilder.Group((uint)group.Id),
            _ => throw new NotImplementedException()
        };
        return FromMessage(builder, entity);
    }
}