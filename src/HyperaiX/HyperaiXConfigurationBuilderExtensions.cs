using System;
using HyperaiX.Clients;
using HyperaiX.Units;
using Microsoft.Extensions.DependencyInjection;

namespace HyperaiX
{
    public static class HyperaiXConfigurationBuilderExtensions
    {
        public static HyperaiXConfigurationBuilder Use<T>(this HyperaiXConfigurationBuilder builder) => builder.Use(typeof(T));
        
        public static HyperaiXConfigurationBuilder UseUnits(this HyperaiXConfigurationBuilder builder) => 
            builder.Use<UnitMiddleware>();
    }
}