using HyperaiX.Clients.Lagrange.Utilties;
using HyperaiX.Extensions.QQ.Roles;
using Lagrange.Core;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Microsoft.Extensions.Caching.Memory;

namespace HyperaiX.Clients.Lagrange.Services;

// 拉格朗日内部已经有缓存，但无法实现缓存项目的单独获取，结合需要转换，这里再次做缓存。
public class MemoryStore(IMemoryCache cache)
{
    public static readonly TimeSpan SlidingExpiration = TimeSpan.FromHours(12);

    public async Task<Group> GetGroupAsync(uint groupId, BotGroup? reference, BotContext factory)
    {
        cache.TryGetValue<Group>(groupId, out var rv);
        if (rv != null) return rv;

        var groups = await factory.FetchGroups();
        foreach (var group in groups)
        {
            var gen = ModelHelper.ToGroup(group.GroupUin, group);
            var members = await factory.FetchMembers(group.GroupUin);
            foreach (var member in members) gen.Members.Add(ModelHelper.ToMember(gen, member.Uin, member));

            cache.Set(gen.Id, gen, SlidingExpiration);
            if (gen.Id == groupId) rv = gen;
        }

        return rv ?? ModelHelper.ToGroup(groupId, reference);
    }

    public async Task<Member> GetMemberAsync(uint groupId, uint memberId, BotGroupMember? reference,
        BotContext factory)
    {
        var group = await GetGroupAsync(groupId, null, factory);
        var member = group.Members.FirstOrDefault(x => x.Id == memberId);
        return member ?? ModelHelper.ToMember(group, memberId, reference);
    }

    public async Task<Friend> GetFriendAsync(uint friendId, BotFriend? reference, BotContext factory)
    {
        cache.TryGetValue<Friend>(friendId, out var rv);
        if (rv != null) return rv;

        var friends = await factory.FetchFriends();
        foreach (var gen in friends.Select(friend => ModelHelper.ToFriend(friend.Uin, friend)))
        {
            cache.Set(gen.Id, gen, SlidingExpiration);
            if (gen.Id == friendId) rv = gen;
        }

        return rv ?? ModelHelper.ToFriend(friendId, reference);
    }
}