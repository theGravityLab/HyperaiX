using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX;

public interface IApiClient
{
    public GenericEventArgs Read();
    public GenericReceipt Write(GenericActionArgs action);
}