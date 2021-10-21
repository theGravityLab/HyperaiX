using System;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Receipts;

namespace HyperaiX
{
    public static class ApiClientExtensions
    {
        public static Task<GenericReceipt> WriteAsync(this IApiClient client, GenericActionArgs args) =>
            Task.Run(() => client.Write(args));
    }
}