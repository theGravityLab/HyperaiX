using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class GroupPokeEventArgs : GenericEventArgs
    {
        public Group Group { get; set; }
        public Member Sender { get; set; }
        public Member Target { get; set; }
    }
}