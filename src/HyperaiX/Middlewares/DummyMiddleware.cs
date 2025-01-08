using HyperaiX.Abstractions.Events;

namespace HyperaiX.Middlewares;

internal class DummyMiddleware : MiddlewareBase
{
    public override void Process(GenericEventArgs _, Action next)
    {
        next();
    }
}