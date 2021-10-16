using System;
using System.Collections;
using System.Collections.Generic;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Clients
{
    public class HyperaiXConfiguration
    {
        public Action<GenericEventArgs, IServiceProvider> Pipeline { get; internal set; }
    }
}