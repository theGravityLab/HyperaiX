using System;

namespace HyperaiX.Abstractions.Events
{
    public abstract class GenericEventArgs: EventArgs
    {
        public DateTime Time { get; set; } = DateTime.Now;
    }
}