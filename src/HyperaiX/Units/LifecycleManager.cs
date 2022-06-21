using System;
using System.Collections.Generic;

namespace HyperaiX.Units;

public class LifecycleManager
{
    private readonly IServiceProvider _provider;
    private readonly IEnumerable<Type> _units;

    public LifecycleManager(IServiceProvider provider, IEnumerable<Type> units)
    {
        _provider = provider;
        _units = units;
    }
}