using HyperaiX.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace HyperaiX.Middlewares;

public class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : MiddlewareBase
{
    public override void Process(GenericEventArgs args, Action next)
    {
        try
        {
            logger.LogInformation("Income: {}", args);
            next();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Income: {} {}", args, ex.Message);
        }
    }
}