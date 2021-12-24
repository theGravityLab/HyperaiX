namespace HyperaiX.Abstractions.Messages;

public abstract record MessageElement
{
    public string TypeName => GetType().Name;
}