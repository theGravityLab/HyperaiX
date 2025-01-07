using HyperaiX.Abstractions.Messages.Payloads;

namespace HyperaiX.Abstractions.Messages;

public record MessageEntity(
    IMessagePayload Body,
    IReadOnlyDictionary<string, object> Attachments,
    DateTimeOffset Timestamp);