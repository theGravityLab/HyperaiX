using HyperaiX.Abstractions.Events;

namespace HyperaiX.Middlewares;

public class BotMiddleware: MiddlewareBase
{
    public override void Process(GenericEventArgs args, Action next)
    {
        next();
    }
}