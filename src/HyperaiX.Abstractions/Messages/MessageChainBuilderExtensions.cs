using System;
using HyperaiX.Abstractions.Messages.ConcreteModels;

namespace HyperaiX.Abstractions.Messages;

public static class MessageChainBuilderExtensions
{
    public static MessageChainBuilder AddPlain(this MessageChainBuilder builder, string text)
    {
        var plain = new Plain(text);
        return builder.Add(plain);
    }

    public static MessageChainBuilder AddImage(this MessageChainBuilder builder, Uri source)
    {
        var image = new Image(source);
        return builder.Add(image);
    }

    public static MessageChainBuilder AddFace(this MessageChainBuilder builder, int faceId)
    {
        var face = new Face(faceId);
        return builder.Add(face);
    }

    public static MessageChainBuilder AddFlash(this MessageChainBuilder builder, Uri source)
    {
        var image = new Flash(source);
        return builder.Add(image);
    }

    public static MessageChainBuilder AddQuote(this MessageChainBuilder builder, string target)
    {
        var quote = new Quote(target);
        return builder.Add(quote);
    }

    public static MessageChainBuilder AddAt(this MessageChainBuilder builder, long who)
    {
        var at = new At(who);
        return builder.Add(at);
    }

    public static MessageChainBuilder AddAtAll(this MessageChainBuilder builder)
    {
        var atAll = new AtAll();
        return builder.Add(atAll);
    }

    public static MessageChainBuilder AddNode(this MessageChainBuilder builder, long userIdentity, string displayName, MessageChain reference)
    {
        var node = new Node(userIdentity, displayName, reference);
        return builder.Add(node);
    }
}