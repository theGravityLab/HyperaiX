namespace HyperaiX.Abstractions.Relations
{
    public abstract record User: RelationModel
    {
        public string Nickname { get; init; }
    }
}