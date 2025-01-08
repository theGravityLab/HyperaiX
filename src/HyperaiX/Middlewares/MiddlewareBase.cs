using HyperaiX.Abstractions.Events;

namespace HyperaiX.Middlewares;

public abstract class MiddlewareBase
{
    public abstract void Process(GenericEventArgs args, Action next);
}