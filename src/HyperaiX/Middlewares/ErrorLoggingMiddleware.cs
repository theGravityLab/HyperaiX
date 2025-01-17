using HyperaiX.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace HyperaiX.Middlewares;

public class ErrorLoggingMiddleware(ILogger<ErrorLoggingMiddleware> logger) : MiddlewareBase
{
    public override void Process(GenericEventArgs args, Action next)
    {
        try
        {
            next();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error caught");
        }
    }
}