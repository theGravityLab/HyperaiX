using System.Threading.Tasks;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Units
{
    public abstract class UnitBase
    {
        public MessageContext Context { get; internal set; }

        public async Task<MessageReceipt> SendGroupMessageAsync(long group, MessageChain chain)
        {
            GroupMessageActionArgs action = new()
            {
                GroupId = group,
                Message = chain
            };
            return await Context.Client.WriteAsync(action) as MessageReceipt;
        }

        public async Task<MessageReceipt> SendFriendMessageAsync(long friend, MessageChain chain)
        {
            FriendMessageActionArgs action = new()
            {
                FriendId = friend,
                Message = chain,
            };
            return await Context.Client.WriteAsync(action) as MessageReceipt;
        }
    }
}