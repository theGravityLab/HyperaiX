using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Units;

public class Session
{
    public bool EndOfLife { get; private set; }
    public IDictionary<string, object> Data { get; } = new Dictionary<string, object>();

    private static readonly List<Session> root = new();

    internal MethodInfo Method { get; init; }
    internal Signature Signature { get; init; }

    private Session(MethodInfo info, Signature signature)
    {
        EndOfLife = false;
        Method = info;
        Signature = signature;

        root.Add(this);
    }

    internal static Session Create(MessageContext context, MethodInfo method, SharingScope scope)
    {
        var session =
            root.FirstOrDefault(x => x.Method == method && context.Sender switch
            {
                Friend it => x.Signature.Match(it),
                Member it => x.Signature.Match(it),
                _ => false
            });
        if (session == default)
        {
            session = new(method, scope switch
            {
                SharingScope.Friend => Signature.FromFriend(context.Sender.Identity),
                SharingScope.Member => Signature.FromMember(context.Group.Identity, context.Sender.Identity),
                SharingScope.Group => Signature.FromGroup(context.Group.Identity)
            });

            root.Add(session);
        }

        return session;
    }

    public T Get<T>(string key, Func<T> factory = null)
    {
        if (Data.ContainsKey(key))
            return (T)Data[key];
        return factory == null ? default : factory();
    }

    public void Set(string key, object obj)
    {
        if (Data.ContainsKey(key)) Data[key] = obj;
        else Data.Add(key, obj);
    }

    public void Finish()
    {
        EndOfLife = true;
        root.Remove(this);
    }
}