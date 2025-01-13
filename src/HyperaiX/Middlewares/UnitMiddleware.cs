using System.Reflection;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Units;
using HyperaiX.Abstractions.Units.Filters;
using HyperaiX.Modules.Features;
using HyperaiX.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX.Middlewares;

public class UnitMiddleware(IServiceProvider provider, IEndClient client, ModuleRegistry registry) : MiddlewareBase
{
    public override void Process(GenericEventArgs args, Action next)
    {
        if (args is MessageEventArgs message)
        {
            var features = registry.GetFeatures<UnitFeature>();

            var context = new MessageContext(client, message.Chat, message.User, message.Message);
            var cache = new Dictionary<Type, UnitBase>();

            foreach (var feature in features)
            foreach (var action in feature.Actions)
            {
                var filters = action.Action.GetCustomAttributes(typeof(FilterAttribute), false);
                var pass = true;
                foreach (var obj in filters)
                    if (obj is FilterAttribute filter)
                    {
                        pass = filter.IsMatched(context, out var value);
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
                        Task.Run(async () => await InvokeAsync(context, unit, action.Action));
                    }
                }
            }
        }
    }

    private async Task InvokeAsync(MessageContext context, UnitBase unit, MethodInfo action)
    {
        // TODO: logging
        var result = action.Invoke(unit, []);
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
    }
}