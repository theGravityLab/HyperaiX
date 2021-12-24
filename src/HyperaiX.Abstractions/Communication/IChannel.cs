using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Abstractions.Communication;

public interface IChannel
{
    void Write(GenericActionArgs action);
    GenericEventArgs Read();
}