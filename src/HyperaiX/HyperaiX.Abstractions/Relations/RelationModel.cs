namespace HyperaiX.Abstractions.Relations
{
    public abstract record RelationModel
    {
        public long Identity { get; init; }
        public virtual string Identifier => Identity.ToString();
    }
}