using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HyperaiX.Abstractions.Events;
using IBuilder;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX.Clients
{
    public class HyperaiXConfigurationBuilder : IBuilder<HyperaiXConfiguration>
    {
        private readonly List<Action<GenericEventArgs, IServiceProvider, Action<GenericEventArgs, IServiceProvider>>>
            middlewares = new();



        public HyperaiXConfigurationBuilder Use(
            Action<GenericEventArgs, IServiceProvider, Action<GenericEventArgs, IServiceProvider>> middleware)
        {
            middlewares.Add(middleware);
            return this;
        }

        public HyperaiXConfigurationBuilder Use(Type type)
        {
            return Use((evt, pvd, next) =>
            {
                var exception =
                    new InvalidOperationException($"{type} has no required method Execute[Async](GenericEventArgs args, Action next)");
                var execute = type.GetMethod("Execute") ?? type.GetMethod("ExecuteAsync") ?? throw exception;
                if (!execute.GetParameters().Select(x => x.ParameterType)
                    .SequenceEqual(new[] { typeof(GenericEventArgs), typeof(Action) })) throw exception;


                var middleware = ActivatorUtilities.CreateInstance(pvd, type);
                Action nextDelegate = () => next(evt, pvd);
                if (execute.GetCustomAttribute<AsyncStateMachineAttribute>() != null)
                {
                    var task = execute.Invoke(middleware, new object[] { evt, nextDelegate }) as Task;
                    task.Wait();
                }
                else
                {
                    execute.Invoke(middleware, new object[] { evt, nextDelegate });
                }

            });
        }

        public HyperaiXConfiguration Build()
        {
            // weave pipeline
            Action<GenericEventArgs, IServiceProvider> pipeline = (args, provider) => { };

            // a, b, c => c, b, a
            // c => c(), empty | pipeline() => c() -> empty()
            // b => b(), c | pipeline() => b() -> c() -> empty()
            // a => a(), b | pipeline() => a() -> b() -> c() -> empty()

            foreach (var m in Enumerable.Reverse(middlewares))
            {
                var _pipeline = pipeline;
                pipeline = (evt, provider) => m(evt, provider, _pipeline);
            }

            return new HyperaiXConfiguration()
            {
                Pipeline = pipeline,
            };
        }
    }
}