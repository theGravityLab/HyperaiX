using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using IBuilder;

namespace HyperaiX.Abstractions.Messages.Builders;

public class RichContentBuilder(MessageEntityBuilder builder) : IBuilder<MessageEntity>
{
    private readonly List<IMessageElement> _elements = [];

    public MessageEntity Build()
    {
        builder.WithPayload(new RichContent(_elements));
        return builder.Build();
    }

    public RichContentBuilder AddElement(IMessageElement element)
    {
        _elements.Add(element);
        return this;
    }
}