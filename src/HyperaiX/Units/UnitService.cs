using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.ConcreteModels;
using HyperaiX.Abstractions.Relations;
using HyperaiX.Units.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Group = System.Text.RegularExpressions.Group;

namespace HyperaiX.Units
{
    public class UnitService
    {
        private readonly UnitServiceConfiguration _configuration;
        private readonly IEnumerable<(Type, MethodInfo)> _methods;
        private readonly IServiceProvider _provider;

        public UnitService(UnitServiceConfiguration configuration, IServiceProvider provider)
        {
            _configuration = configuration;
            _methods = _configuration.Units.SelectMany(t =>
                t.GetMethods()
                    .Where(m => m.IsPublic && !m.IsAbstract && !m.IsStatic && !m.IsConstructor)
                    .Select(m => (t, m)));
            _provider = provider;
        }

        public void Push(MessageContext context)
        {
            // compile message chain
            var message = context.Message;
            // TODO: escape curly braces
            // !echo --message {1:Image}
            var flatten = message.Flatten();

            foreach (var (type, method) in _methods)
            {
                var receiver = method.GetCustomAttribute<ReceiverAttribute>();
                if (!(receiver != null && (receiver.Type & context.Type) > MessageEventType.None))
                {
                    continue;
                }

                var command = method.GetCustomAttribute<CommandAttribute>();
                if (command != null)
                {
                    ProcessCommand(context, type, method, command, flatten);
                }

                var handler = method.GetCustomAttribute<HandlerAttribute>();
                if (handler != null)
                {
                    ProcessHandler(context, type, method, handler, flatten);
                }
            }
        }

        private void ProcessCommand(MessageContext context, Type type, MethodInfo method, CommandAttribute command,
            string flatten)
        {
            //TODO: use command parse lib instead
            //var options = method.GetCustomAttributes<OptionAttribute>();
        }

        private void ProcessHandler(MessageContext context, Type type, MethodInfo method, HandlerAttribute handler,
            string flatten)
        {
            Match match = handler.Compiled.Match(flatten);
            if (match.Success)
            {
                var properties = new Dictionary<string, MessageChain>();
                foreach (Group group in match.Groups)
                {
                    if (group.Success && group.Name != string.Empty)
                    {
                        var key = group.Name;
                        var value = group.Value;

                        properties.Add(key, context.Message.Extract(value));
                    }
                }
                var arguments = PrepareArguments(context, properties, method);
                var unit = ActivatorUtilities.CreateInstance(_provider, type) as UnitBase;
                unit.Context = context;
                InvokeMethod(method, unit, arguments);
            }
        }

        private object[] PrepareArguments(MessageContext context, IReadOnlyDictionary<string, MessageChain> properties,
            MethodInfo method)
        {
            //MessageContext by type
            //properties by name, primarily, auto-convert type

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
                else if(contextTypes.ContainsKey(parameter.ParameterType))
                {
                    arguments[count] = contextTypes[parameter.ParameterType];
                }
                else
                {
                    throw new ArgumentNullException($"{parameter.Name}({parameter.ParameterType.Name}) not provided");
                }
                count++;
            }
            return arguments;
        }

        private void InvokeMethod(MethodInfo method, UnitBase unit, object[] arguments)
        {
            //TODO: support custom return type
            if (method.GetCustomAttribute<AsyncStateMachineAttribute>() != null)
            {
                Task.Run(() => method.Invoke(unit, arguments)).ConfigureAwait(false);
            }
            else
            {
                method.Invoke(unit, arguments);
            }
        }
    }
}