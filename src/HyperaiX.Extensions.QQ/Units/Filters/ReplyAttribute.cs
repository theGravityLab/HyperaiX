using Duffet.Builders;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Units;
using HyperaiX.Abstractions.Units.Filters;
using HyperaiX.Extensions.QQ.Messages;

namespace HyperaiX.Extensions.QQ.Units.Filters;

public class ReplyAttribute : FilterAttribute
{
    public override bool IsMatched(MessageContext context, IBankBuilder bank)
    {
        if (context.Message.Attachments.TryGetValue(MessageEntityExtensions.ATTACHMENT_REFERENCE, out var value) &&
            value is ulong messageId)
        {
            // 目前做不到，拉格朗日没法获取消息
            bank.Property().Typed(typeof(MessageEntity)).WithLazy(new Lazy<object>(() => throw new NotImplementedException(), true));
            return true;
        }

        return false;
    }
}