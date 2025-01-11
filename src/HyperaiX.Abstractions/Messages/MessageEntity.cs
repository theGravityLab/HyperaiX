using HyperaiX.Abstractions.Messages.Payloads;

namespace HyperaiX.Abstractions.Messages;

public record MessageEntity(
    string Preview,
    IMessagePayload Body,
    IReadOnlyDictionary<string, object> Attachments,
    DateTimeOffset Timestamp);