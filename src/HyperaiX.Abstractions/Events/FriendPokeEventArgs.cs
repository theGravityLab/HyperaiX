using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class FriendPokeEventArgs: GenericEventArgs
    {
        public Friend Sender { get; set; }
    }
}