using System.Diagnostics;
using HyperaiX.Abstractions.Bots;
using HyperaiX.Abstractions.Events;
using HyperaiX.Modules.Features;
using HyperaiX.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HyperaiX.Middlewares;

public class BotMiddleware(IServiceProvider provider, ModuleRegistry registry, ILogger<BotMiddleware> logger)
    : MiddlewareBase
{
    public override void Process(GenericEventArgs args, Action next)
    {
        var features = registry.GetFeatures<BotFeature>();


        var bots = new List<BotBase>();

        foreach (var feature in features)
        {
            if (feature.ActivatedBots is null)
            {
                feature.ActivatedBots = new List<BotBase>();
                foreach (var type in feature.BotTypes)
                {
                    var bot = (BotBase)ActivatorUtilities.CreateInstance(provider, type);
                    feature.ActivatedBots.Add(bot);
                }
            }

            foreach (var bot in feature.ActivatedBots)
                bots.Add(bot);
        }

        if (bots.Count != 0) _ = Task.Run(() => Emit(bots, args));
        next();
    }

    private Task Emit(IEnumerable<BotBase> bots, GenericEventArgs args)
    {
        var tasks = bots.Select(async x =>
        {
            try
            {
                await x.OnEventAsync(args);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "The bot {} has ran into exception", x.GetType().Name);
            }
        });
        return Task.WhenAll(tasks);
    }
}