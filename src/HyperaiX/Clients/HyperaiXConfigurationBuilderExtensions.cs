using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HyperaiX.Clients
{
    public static class HyperaiXConfigurationBuilderExtensions
    {

        public static HyperaiXConfigurationBuilder UseLogging(this HyperaiXConfigurationBuilder builder) =>
            builder.Use((evt, pvd, nxt) =>
            {
                var logger = pvd.GetRequiredService<ILogger<HyperaiXConfigurationBuilder>>();
                logger.LogInformation("{}", evt);
                nxt(evt, pvd);
            });
    }
}