namespace HyperaiX.Abstractions.Messages.ConcreteModels;

public sealed record Unknown : MessageElement
{
    public Unknown(object data)
    {
        Data = data;
    }

    public object Data { get; init; }
}