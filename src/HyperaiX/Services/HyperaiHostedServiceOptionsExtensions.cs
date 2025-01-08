using HyperaiX.Middlewares;

namespace HyperaiX.Services;

public static class HyperaiHostedServiceOptionsExtensions
{

    public static HyperaiHostedServiceOptions UseLogging(this HyperaiHostedServiceOptions self)
    {
        self.Use<LoggingMiddleware>();
        return self;
    }

    public static HyperaiHostedServiceOptions UseBlacklist(this HyperaiHostedServiceOptions self)
    {
        self.Use<BlacklistMiddleware>();
        return self;
    }

    public static HyperaiHostedServiceOptions UseBots(this HyperaiHostedServiceOptions self)
    {
        self.Use<BotMiddleware>();
        return self;
    }
}