using HyperaiX.Abstractions.Messages.Payloads;
using IBuilder;

namespace HyperaiX.Abstractions.Messages.Builders;

public class PayloadValueBuilder(IMessagePayload payload) : IBuilder<IMessagePayload>
{
    public IMessagePayload Build()
    {
        return payload;
    }
}