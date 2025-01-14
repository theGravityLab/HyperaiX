using System.Diagnostics;
using System.Reflection;
using Duffet.Builders;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Units;
using HyperaiX.Abstractions.Units.Filters;
using HyperaiX.Modules.Features;
using HyperaiX.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyperaiX.Middlewares;

public class UnitMiddleware(
    IOptions<UnitMiddlewareOptions> options,
    IServiceProvider provider,
    IEndClient client,
    ModuleRegistry registry,
    ILogger<UnitMiddleware> logger) : MiddlewareBase
{
    public override void Process(GenericEventArgs args, Action next)
    {
        if (args is MessageEventArgs message)
        {
            var features = registry.GetFeatures<UnitFeature>();

            var context = new MessageContext(client, message.Chat, message.Sender, message.Self, message.Message);
            var cache = new Dictionary<Type, UnitBase>();

            foreach (var feature in features)
            foreach (var action in feature.Actions)
            {
                var filters = action.Action.GetCustomAttributes(typeof(FilterAttribute), false);
                var builder = new LazyBankBuilder();
                var pass = true;
                foreach (var obj in filters)
                    if (obj is FilterAttribute filter)
                    {
                        pass = filter.IsMatched(context, builder);
                        if (!pass) break;
                    }

                if (pass)
                {
                    if (!cache.TryGetValue(action.Unit, out var unit))
                    {
                        unit = ActivatorUtilities.CreateInstance(provider, action.Unit) as UnitBase;
                        if (unit != null) cache.Add(action.Unit, unit);
                    }

                    if (unit != null)
                    {
                        unit.Context = context;
                        var bank = builder.Build();
                        var arguments = bank.Serve(action.Action);
                        Task.Run(async () => await InvokeAsync(context, unit, action.Action, arguments));
                    }
                }
            }
        }
    }

    private async Task InvokeAsync(MessageContext context, UnitBase unit, MethodInfo action, object[] arguments)
    {
        try
        {
            logger.LogInformation("Action hit {}:{}", unit.GetType().Name, action.Name);
            var watch = new Stopwatch();
            watch.Start();
            var result = action.Invoke(unit, arguments);
            switch (result)
            {
                case Task<MessageEntity?> entityTask:
                {
                    var entity = await entityTask;
                    if (entity is not null)
                        await client.WriteAsync(new SendMessageActionArgs(context.Chat, entity));
                    break;
                }
                case Task task:
                    await task;
                    break;
                case ValueTask<MessageEntity?> entityValue:
                {
                    var entity = await entityValue;
                    if (entity is not null)
                        await client.WriteAsync(new SendMessageActionArgs(context.Chat, entity));
                    break;
                }
                case ValueTask value:
                    await value;
                    break;
                case MessageEntity entity:
                {
                    await client.WriteAsync(new SendMessageActionArgs(context.Chat, entity));
                    break;
                }
            }

            watch.Stop();
            logger.LogInformation("Action {}:{} finished in {}ms", unit.GetType().Name, action.Name,
                watch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unit {}:{} has ran into exception", unit.GetType().Name, action.Name);
        }
    }
}