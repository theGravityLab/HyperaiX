using HyperaiX.Abstractions.Messages.Builders;
using HyperaiX.Abstractions.Messages.Payloads;

namespace HyperaiX.Abstractions.Messages;

public record MessageEntity(
    string Preview,
    IMessagePayload Body,
    IReadOnlyDictionary<string, object> Attachments,
    DateTimeOffset Timestamp)
{
    public static MessageEntityBuilder Builder()
    {
        return new MessageEntityBuilder();
    }

    public static MessageEntity CreateText(string text)
    {
        var builder = Builder();
        builder.RichContent().Text(text);

        return builder.WithPreview(text).Build();
    }
}