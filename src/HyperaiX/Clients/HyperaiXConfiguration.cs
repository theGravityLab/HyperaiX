using System;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Clients;

public class HyperaiXConfiguration
{
    public Action<GenericEventArgs, IServiceProvider> Pipeline { get; internal set; }
}