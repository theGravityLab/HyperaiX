namespace HyperaiX.Abstractions.Messages.ConcreteModels;

public sealed record Node : MessageElement
{
    public Node(long userIdentity, string userDisplayName, MessageChain chain)
    {
        UserIdentity = UserIdentity;
        UserDisplayName = userDisplayName;
        Message = chain;
    }

    public long UserIdentity { get; init; }
    public string UserDisplayName { get; init; }
    public MessageChain Message { get; init; }
}