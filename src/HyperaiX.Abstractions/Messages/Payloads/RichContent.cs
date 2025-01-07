using HyperaiX.Abstractions.Messages.Payloads.Elements;

namespace HyperaiX.Abstractions.Messages.Payloads;

public record RichContent(IReadOnlyList<IMessageElement> Elements) : IMessagePayload;