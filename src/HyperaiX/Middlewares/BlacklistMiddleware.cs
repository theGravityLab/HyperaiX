using HyperaiX.Abstractions.Events;

namespace HyperaiX.Middlewares;

public class BlacklistMiddleware: MiddlewareBase
{
    public override void Process(GenericEventArgs args, Action next)
    {
        next();
    }
}