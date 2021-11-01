using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Events;
using HyperaiX.Units.Attributes;

namespace HyperaiX.Units
{
    public class UnitService
    {
        private readonly UnitServiceConfiguration _configuration;

        private readonly IEnumerable<(Type, MethodInfo)> _methods;

        public UnitService(UnitServiceConfiguration configuration)
        {
            _configuration = configuration;
            _methods = _configuration.Units.SelectMany(t =>
                t.GetMethods()
                .Where(m => m.IsPublic && !m.IsAbstract && !m.IsStatic && !m.IsConstructor)
                .Select(m => (t, m)));
        }

        public void Push(GenericEventArgs args)
        {
            var messageType = args switch
            {
                GroupMessageEventArgs => MessageEventType.Group,
                FriendMessageEventArgs => MessageEventType.Friend,
                _ => MessageEventType.None
            };
            foreach(var (type, method) in _methods)
            {
                var receiver = method.GetCustomAttribute<ReceiverAttribute>();
                if(!(receiver != null && (receiver.Type & messageType) > MessageEventType.None))
                {
                    continue;
                }
                var command = method.GetCustomAttribute<CommandAttribute>();
                if(command != null)
                {
                    ProcessCommand(args, type, method, command);
                }
                var handler = method.GetCustomAttribute<HandlerAttribute>();
                if(handler != null)
                {
                    ProcessHandler(args, type, method, handler);
                }
            }
        }

        private void ProcessCommand(GenericEventArgs args, Type type, MethodInfo method, CommandAttribute command)
        {
            //TODO:
        }

        private void ProcessHandler(GenericEventArgs args, Type type, MethodInfo method, HandlerAttribute handler)
        {
            //TODO:
        }
    }
}