namespace HyperaiX.Abstractions.Relations
{
    public record Friend : User
    {
        public string Remark { get; init; }
    }
}