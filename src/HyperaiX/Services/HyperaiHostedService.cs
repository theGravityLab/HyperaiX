﻿using Microsoft.Extensions.Hosting;

namespace HyperaiX.Services;

public class HyperaiHostedService: IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}