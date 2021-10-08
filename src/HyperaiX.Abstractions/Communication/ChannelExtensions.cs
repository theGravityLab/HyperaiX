using System.Threading;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Abstractions.Communication
{
    public static class ChannelExtensions
    {
        public static Task WriteAsync(this IChannel channel, GenericActionArgs action)
        {
            return Task.Run(() => channel.Write(action));
        }

        public static Task<GenericEventArgs> ReadAsync(this IChannel channel)
        {
            return Task.Run(channel.Read);
        }
    }
}