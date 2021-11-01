using System.Linq;
using System.Runtime.Loader;
using HyperaiX.Units;

namespace HyperaiX.Units
{
    public static class UnitServiceConfigurationBuilderExtensions
    {
        public static UnitServiceConfigurationBuilder MapUnits(this UnitServiceConfigurationBuilder builder)
        {
            var entries = AssemblyLoadContext.All.SelectMany(x => x.Assemblies).SelectMany(x => x.GetExportedTypes())
                .Where(x => x.IsAssignableTo(typeof(UnitBase)));

            foreach (var entry in entries)
            {
                builder.AddUnit(entry);
            }

            return builder;
        }
    }
}