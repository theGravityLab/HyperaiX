using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Duffet;

namespace HyperaiX.Units;

public class UnitService
{
    private readonly UnitServiceConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IEnumerable<(Type, MethodInfo)> _methods;
    private readonly IServiceProvider _provider;

    private readonly Dictionary<MethodInfo, Session> sessionTable = new();

    public UnitService(UnitServiceConfiguration configuration, IServiceProvider provider, ILogger<UnitService> logger)
    {
        _configuration = configuration;
        _methods = _configuration.Units.SelectMany(t =>
            t.GetMethods()
                .Where(m => m.IsPublic && !m.IsAbstract && !m.IsStatic && !m.IsConstructor)
                .Select(m => (t, m)));
        _provider = provider;
        _logger = logger;
    }

    public void Pussy(MessageContext context)
    {
        foreach (var (type, method) in _methods)
        {
            var receiver = method.GetCustomAttribute<ReceiverAttribute>();
            if (!(receiver != null && (receiver.Type & context.Type) > MessageEventType.None)) continue;

            var extractors = method.GetCustomAttributes<ActionFieldAttributeBase>(); // 取 any, 也就是都要跑一边，不管逻辑ALL也不逻辑短路
            foreach (var extractor in extractors) LickPussy(context, extractor, method, type); //TODO: 有过滤器就按照过滤器挨个执行，如果没有过滤器就不提供 MessageChain 候选执行一次
        }
    }

    private void LickPussy(MessageContext context, ActionFieldAttributeBase extractor, MethodInfo method, Type type)
    {
        var success = extractor.Match(context, out var properties);
        if (success)
        {
            var builder = Bank.Builder();
            foreach (var property in properties)
            {
                builder.Property().Named(property.Key).Typed(typeof(MessageChain))
                    .HasTypeAdapted(typeof(MessageChain), (it, type) => it)
                    .HasTypeAdapted(typeof(MessageElement), (it, type) => ((MessageChain)it).First(x => x.GetType() == type))
                    .HasTypeAdapted(typeof(string), (it, type) => it.ToString());
            }
            var unit = ActivatorUtilities.CreateInstance(_provider, type) as UnitBase;
            unit.Context = context;
            WrapPussy(method, unit, builder.Build().Serve(method), context);
        }
    }

    [Obsolete]
    private object[] PrepareInjectionsForPussy(MessageContext context, IReadOnlyDictionary<string, MessageChain> properties,
        MethodInfo method)
    {
        //MessageContext by type
        //properties by name, primarily, auto-converting type

        var parameters = method.GetParameters();
        var arguments = new object[parameters.Length];

        var contextTypes = new Dictionary<Type, object>(context.GetType().GetProperties()
            .Select(x => new KeyValuePair<Type, object>(x.PropertyType, x.GetValue(context))));
        var count = 0;
        foreach (var parameter in parameters)
        {
            if (properties.ContainsKey(parameter.Name!))
            {
                var obj = properties[parameter.Name];
                var type = parameter.ParameterType;
                arguments[count] = parameter.ParameterType switch
                {
                    _ when type == typeof(MessageChain) => obj,
                    _ when type.IsAssignableTo(typeof(MessageElement)) => obj.First(x => x.GetType() == type),
                    _ when type == typeof(string) => obj.ToString(),
                    _ => throw new ArgumentOutOfRangeException(null,
                        "Only support MessageElement, MessageChain, string")
                };
            }
            else if (contextTypes.ContainsKey(parameter.ParameterType))
            {
                arguments[count] = contextTypes[parameter.ParameterType];
            }
            else if (method.GetCustomAttribute<PersistenceAttribute>() != null &&
                     parameter.ParameterType == typeof(Session))
            {
                var session = sessionTable.ContainsKey(method) && !sessionTable[method].EndOfLife
                    ? sessionTable[method]
                    : new Session();
                arguments[count] = session;
            }
            else
            {
                throw new ArgumentNullException($"{parameter.Name}({parameter.ParameterType.Name}) not provided");
            }

            count++;
        }

        return arguments;
    }

    private void WrapPussy(MethodInfo method, UnitBase unit, object[] arguments, MessageContext context)
    {
        if (method.GetCustomAttribute<AsyncStateMachineAttribute>() != null)
            Task.Run(async () =>
            {
                if (method.Invoke(unit, arguments) is not Task task) return;
                await task.ConfigureAwait(false);
                var result = task.GetType().GetProperty("Result").GetValue(task);
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
            }).ConfigureAwait(false);
        else
            Task.Run(async () =>
            {
                if (method.Invoke(unit, arguments) is not object result) return;
                await ForwardActionResultAsync(result, context);
            });

        async Task ForwardActionResultAsync(object result, MessageContext context)
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
                _ => throw new NotImplementedException()
            };
            await context.SendMessageAsync(chain);
            _logger.LogInformation("Forwarded by ReturnType({Type} to {Type})", result.GetType().Name, context.Type);
        }
    }
}