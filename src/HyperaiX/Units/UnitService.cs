using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Duffet;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HyperaiX.Units;

public class UnitService
{
    private readonly UnitServiceConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IEnumerable<(Type, MethodInfo)> _methods;
    private readonly IServiceProvider _provider;

    private readonly LifecycleManager manager;

    public UnitService(UnitServiceConfiguration configuration, IServiceProvider provider, ILogger<UnitService> logger)
    {
        _configuration = configuration;
        _methods = _configuration.Units.SelectMany(t =>
            t.GetMethods()
                .Where(m => m.IsPublic && !m.IsAbstract && !m.IsStatic && !m.IsConstructor)
                .Select(m => (t, m)));
        _provider = provider;
        _logger = logger;

        manager = new LifecycleManager(_provider, _methods.Select(x => x.Item1));
    }

    public void Push(MessageContext context)
    {
        foreach (var (type, method) in _methods)
        {
            var receiver = method.GetCustomAttribute<ReceiverAttribute>();
            if (receiver != null && (receiver.Type & context.Type) > MessageEventType.None)
                DoReceiver(context, type, method);
        }
    }

    private void DoReceiver(MessageContext context, Type type, MethodInfo method)
    {
        var builder = Bank.Builder();
        var extractors = method.GetCustomAttributes<ActionFieldAttributeBase>().ToArray();
        var success = extractors.Length == 0 || extractors.All(action =>
        {
            var result = action.Match(context, out var dict);
            if (result)
                foreach (var d in dict)
                    builder.Property()
                        .Named(d.Key)
                        .Typed(typeof(MessageChain))
                        .WithObject(d.Value)
                        .HasTypeAdapted(typeof(MessageChain), (it, _) => it)
                        .HasTypeAdapted(typeof(MessageElement),
                            (it, t) => ((MessageChain)it).First(x => x.GetType() == t))
                        .HasTypeAdapted(typeof(string), (it, _) => it.ToString());

            return result;
        });
        if (success)
        {
            _logger.LogDebug("Unit action triggered: {Method}@{Unit}", method.Name, type.Name);
            foreach (var (t, value) in context.GetType().GetProperties()
                         .Select(x => (x.PropertyType, x.GetValue(context))))
                builder.Property().Typed(t).HasTypeAdapted(value.GetType(), (o, t) => o).WithObject(value);

            var persistence = method.GetCustomAttribute<PersistenceAttribute>();
            if (persistence != null)
            {
                var scope = persistence.Scope;
                var session = Session.Create(context, method, scope);

                builder.Property()
                    .Typed(typeof(Session))
                    .WithObject(session);
            }

            ExecuteAsyncUnit(method, type, builder.Build(), context);
        }
    }

    private void ExecuteAsyncUnit(MethodInfo method, Type unitType, Bank bank, MessageContext context)
    {
        var arguments = bank.Serve(method);
        if (method.GetCustomAttribute<AsyncStateMachineAttribute>() != null)
            Task.Run(async () =>
            {
                await using var scope = _provider.CreateAsyncScope();
                var unit = ActivatorUtilities.CreateInstance(_provider, unitType) as UnitBase;
                unit!.Context = context;
                if (method.Invoke(unit, arguments) is not Task task) return;
                await task.ConfigureAwait(false);
                var result = task.GetType().GetProperty("Result")?.GetValue(task);
                if (result != null)
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        await ForwardActionResultAsync(result, context);
                    }
                    else
                    {
                        var exception = task.Exception;
                        _logger.LogError(exception, "Exception caught while running async method: {MethodName}",
                            method.Name);
                    }
                }
            }).ConfigureAwait(false);
        else
            Task.Run(async () =>
            {
                await using var scope = _provider.CreateAsyncScope();
                var unit = ActivatorUtilities.CreateInstance(_provider, unitType) as UnitBase;
                unit!.Context = context;
                if (method.Invoke(unit, arguments) is not { } result) return;
                await ForwardActionResultAsync(result, context);
            }).ConfigureAwait(false);
    }

    private async Task ForwardActionResultAsync(object result, MessageContext context)
    {
        // MessageElement
        // MessageChainBuilder
        // string
        // StringBuilder
        // IEnumerable<MessageElement>
        var chain = result switch
        {
            MessageChain it => it,
            string it => MessageChain.Construct(new Plain(it)),
            StringBuilder it => MessageChain.Construct(new Plain(it.ToString())),
            IEnumerable<MessageElement> it => new MessageChain(it),
            MessageChainBuilder it => it.Build(),
            MessageElement it => MessageChain.Construct(it),
            _ => MessageChain.Construct(new Plain(result.ToString()))
        };
        await context.SendAsync(chain);
        _logger.LogDebug("Forwarded by ReturnType({Type} to {Message})", result.GetType(), context.Type);
    }
}