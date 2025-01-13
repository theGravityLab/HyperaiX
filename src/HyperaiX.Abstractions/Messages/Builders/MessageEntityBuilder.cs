using HyperaiX.Abstractions.Messages.Payloads;
using IBuilder;

namespace HyperaiX.Abstractions.Messages.Builders;

public class MessageEntityBuilder : IBuilder<MessageEntity>
{
    private readonly Dictionary<string, object> _attachments = new();
    private IBuilder<IMessagePayload>? _payload;
    private string? _preview;

    public MessageEntity Build()
    {
        var payload = _payload?.Build() ?? new Unknown();
        return new MessageEntity(_preview ?? payload.ToString() ?? string.Empty, payload, _attachments,
            DateTimeOffset.Now);
    }

    public MessageEntityBuilder WithPreview(string preview)
    {
        _preview = preview;
        return this;
    }

    public MessageEntityBuilder WithPayload(IMessagePayload payload)
    {
        _payload = new PayloadValueBuilder(payload);
        return this;
    }

    public MessageEntityBuilder WithPayload(IBuilder<IMessagePayload> paylaod)
    {
        _payload = paylaod;
        return this;
    }

    public MessageEntityBuilder AddAttachment(string key, object value)
    {
        _attachments.Add(key, value);
        return this;
    }
}