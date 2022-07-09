using System.Threading.Tasks;
using HyperaiX.Abstractions.Messages;

namespace HyperaiX.Units;

public static class MessageContextExtensions
{
    public static async Task ReplyAsync(this MessageContext context, MessageChain chain)
    {
        var reply = context.Message.CanBeReplied()
            ? context.Message.MakeReply().AddRange(chain).Build()
            : new MessageChainBuilder().AddRange(chain).Build();
        await SendAsync(context, reply);
    }

    public static async Task SendAsync(this MessageContext context, MessageChain chain)
    {
        if (context.Group == null)
            await context.Client.SendFriendMessageAsync(context.Sender.Identity, chain);
        else
            await context.Client.SendGroupMessageAsync(context.Group.Identity, chain);
    }
}