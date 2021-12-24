namespace HyperaiX.Abstractions.Messages.ConcreteModels;

public sealed record Plain : MessageElement
{
    public Plain(string text)
    {
        Text = text;
    }

    public string Text { get; init; }
}