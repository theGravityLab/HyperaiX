using System.Threading.Tasks;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX;

public static class ApiClientExtensions
{
    public static async Task<GenericReceipt> WriteAsync(this IApiClient client, GenericActionArgs args)
    {
        return await Task.Run(() => client.Write(args));
    }

    public static async Task<long> SendFriendMessageAsync(this IApiClient client, long friendId, MessageChain chain)
    {
        var args = new FriendMessageActionArgs
        {
            FriendId = friendId,
            Message = chain
        };
        var receipt = await client.WriteAsync(args) as MessageReceipt;
        return receipt.MessageId;
    }

    public static async Task<long> SendGroupMessageAsync(this IApiClient client, long groupId, MessageChain chain)
    {
        var args = new GroupMessageActionArgs
        {
            GroupId = groupId,
            Message = chain
        };
        var receipt = await client.WriteAsync(args) as MessageReceipt;
        return receipt.MessageId;
    }

    public static async Task<Group> GetGroupInfoAsync(this IApiClient client, long groupId)
    {
        var args = new QueryGroupActionArgs
        {
            GroupId = groupId
        };
        var receipt = await client.WriteAsync(args) as QueryGroupReceipt;
        return receipt.Group;
    }

    public static async Task<Friend> GetFriendInfoAsync(this IApiClient client, long friendId)
    {
        var args = new QueryFriendActionArgs
        {
            FriendId = friendId
        };
        var receipt = await client.WriteAsync(args) as QueryFriendReceipt;
        return receipt.Friend;
    }

    public static async Task<Member> GetMemberInfoAsync(this IApiClient client, long groupId, long memberId)
    {
        var args = new QueryMemberActionArgs
        {
            GroupId = groupId,
            MemberId = memberId
        };
        var receipt = await client.WriteAsync(args) as QueryMemberReceipt;
        return receipt.Member;
    }
}