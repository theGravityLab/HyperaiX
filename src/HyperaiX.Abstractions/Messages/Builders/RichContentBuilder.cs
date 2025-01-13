using HyperaiX.Abstractions.Messages.Payloads;
using HyperaiX.Abstractions.Messages.Payloads.Elements;
using IBuilder;

namespace HyperaiX.Abstractions.Messages.Builders;

public class RichContentBuilder : IBuilder<IMessagePayload>
{
    private readonly List<IMessageElement> _elements = [];

    public IMessagePayload Build()
    {
        return new RichContent(_elements);
    }

    public RichContentBuilder AddElement(IMessageElement element)
    {
        _elements.Add(element);
        return this;
    }
}