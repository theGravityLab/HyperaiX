using System;
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
        return await client.WriteAsync(args) switch
        {
            MessageReceipt receipt => receipt.MessageId,
            _ => default,
        };
    }

    public static async Task<long> SendGroupMessageAsync(this IApiClient client, long groupId, MessageChain chain)
    {
        var args = new GroupMessageActionArgs
        {
            GroupId = groupId,
            Message = chain
        };
        return await client.WriteAsync(args) switch
        {
            MessageReceipt receipt => receipt.MessageId,
            _ => default,
        };
    }

    public static async Task<Group> GetGroupInfoAsync(this IApiClient client, long groupId)
    {
        var args = new QueryGroupActionArgs
        {
            GroupId = groupId
        };
        return await client.WriteAsync(args) switch
        {
            QueryGroupReceipt receipt => receipt.Group,
            _ => default,
        };
    }

    public static async Task<Friend> GetFriendInfoAsync(this IApiClient client, long friendId)
    {
        var args = new QueryFriendActionArgs
        {
            FriendId = friendId
        };
        return await client.WriteAsync(args) switch
        {
            QueryFriendReceipt receipt => receipt.Friend,
            _ => default,
        };
    }

    public static async Task<Member> GetMemberInfoAsync(this IApiClient client, long groupId, long memberId)
    {
        var args = new QueryMemberActionArgs
        {
            GroupId = groupId,
            MemberId = memberId
        };
        return await client.WriteAsync(args) switch
        {
            QueryMemberReceipt receipt => receipt.Member,
            _ => default,
        };
    }

    public static async Task<MessageChain> GetMessageAsync(this IApiClient client, long messageId)
    {
        var args = new QueryMessageActionArgs()
        {
            MessageId = messageId
        };
        return await client.WriteAsync(args) switch
        {
            QueryMessageReceipt receipt => receipt.Message,
            _ => default,
        };
    }

    public static async Task<Self> GetSelfInfoAsync(this IApiClient client)
    {
        var args = new QuerySelfActionArgs();
        return await client.WriteAsync(args) switch
        {
            QuerySelfReceipt receipt => receipt.Info,
            _ => default
        };
    }
}