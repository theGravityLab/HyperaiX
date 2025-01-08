using HyperaiX.Middlewares;

namespace HyperaiX.Services;

public class HyperaiHostedServiceOptions
{
    public IList<Type> Middlewares { get; } = new List<Type>();

    public HyperaiHostedServiceOptions Use<T>()
        where T : MiddlewareBase
    {
        Middlewares.Add(typeof(T));
        return this;
    }
}