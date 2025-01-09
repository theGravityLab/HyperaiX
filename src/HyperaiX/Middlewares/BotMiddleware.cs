using HyperaiX.Abstractions.Bots;
using HyperaiX.Abstractions.Events;
using HyperaiX.Modules;
using HyperaiX.Modules.Features;
using HyperaiX.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HyperaiX.Middlewares;

public class BotMiddleware(IServiceProvider provider, ModuleRegistry registry) : MiddlewareBase
{
    public override void Process(GenericEventArgs args, Action next)
    {
        var features = registry.GetFeatures<BotFeature>();

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
            {
                Task.Run(async () => await bot.OnEventAsync(args)).ContinueWith(t =>
                {
                    // TODO: logging if faulted
                });
            }
        }

        next();
    }
}