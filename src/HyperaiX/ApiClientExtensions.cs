using System;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX
{
    public static class ApiClientExtensions
    {
        public async static Task<GenericReceipt> WriteAsync(this IApiClient client, GenericActionArgs args) =>
            await Task.Run(() => client.Write(args));

        public async static Task<long> SendFriendMessageAsync(this IApiClient client, long friendId, MessageChain chain)
        {
            var args = new FriendMessageActionArgs()
            {
                FriendId = friendId,
                Message = chain
            };
            var receipt = await client.WriteAsync(args) as MessageReceipt;
            return receipt.MessageId;
        }

        public async static Task<long> SendGroupMessageAsync(this IApiClient client, long groupId, MessageChain chain)
        {
            var args = new GroupMessageActionArgs()
            {
                GroupId = groupId,
                Message = chain
            };
            var receipt = await client.WriteAsync(args) as MessageReceipt;
            return receipt.MessageId;
        }

        public async static Task<Group> GetGroupInfoAsync(this IApiClient client, long groupId)
        {
            var args = new QueryGroupActionArgs()
            {
                GroupId = groupId
            };
            var receipt = await client.WriteAsync(args) as QueryGroupReceipt;
            return receipt.Group;
        }

        public async static Task<Friend> GetFriendInfoAsync(this IApiClient client, long friendId)
        {
            var args = new QueryFriendActionArgs()
            {
                FriendId = friendId
            };
            var receipt = await client.WriteAsync(args) as QueryFriendReceipt;
            return receipt.Friend;
        }

        public async static Task<Member> GetMemberInfoAsync(this IApiClient client, long groupId, long memberId)
        {
            var args = new QueryMemberActionArgs()
            {
                GroupId = groupId,
                MemberId = memberId
            };
            var receipt = await client.WriteAsync(args) as QueryMemberReceipt;
            return receipt.Member;
        }
    }
}