using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Abstractions.Events
{
    public class MemberCardChangeEventArgs : GenericEventArgs
    {
        public Group Group { get; set; }
        public string Previous { get; set; }
        public string Present { get; set; }
    }
}