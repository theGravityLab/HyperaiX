namespace HyperaiX.Abstractions.Messages
{
    public interface IMessageChainParser
    {
        MessageChain Parse(string text);
    }
}