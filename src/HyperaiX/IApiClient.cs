using System.Threading;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX;

public interface IApiClient
{
    public GenericEventArgs Read(CancellationToken token);
    public GenericReceipt Write(GenericActionArgs action);
}