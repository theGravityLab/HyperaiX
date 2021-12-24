namespace HyperaiX.Abstractions.Messages;

public interface IMessageChainFormatter
{
    string Format(MessageChain chain);
}