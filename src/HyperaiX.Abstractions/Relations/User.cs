namespace HyperaiX.Abstractions.Relations;

public abstract record User : Contact
{
    public string Nickname { get; init; }
}