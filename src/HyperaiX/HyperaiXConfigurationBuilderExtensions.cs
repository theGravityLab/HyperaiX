using HyperaiX.Clients;
using HyperaiX.Units;

namespace HyperaiX;

public static class HyperaiXConfigurationBuilderExtensions
{
    public static HyperaiXConfigurationBuilder Use<T>(this HyperaiXConfigurationBuilder builder)
    {
        return builder.Use(typeof(T));
    }

    public static HyperaiXConfigurationBuilder UseUnits(this HyperaiXConfigurationBuilder builder)
    {
        return builder.Use<UnitMiddleware>();
    }
}