using System;

namespace HyperaiX.Abstractions.Actions
{
    public abstract class GenericActionArgs
    {
        public virtual Type Output => typeof(GenericActionArgs);
    }
}