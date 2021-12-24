namespace HyperaiX.Abstractions.Relations;

public abstract record Contact
{
    public long Identity { get; init; }
    public virtual string Identifier => Identity.ToString();
}