using System.Collections.Generic;
using System.Reflection;

namespace CrudMaster
{
    public static class Config
    {
        public static List<Assembly> AssembliesForMapping { get; set; }

        public static Assembly AppAssembly { get; set; }
    }

    public class CrudMastedConfig
    {
        public void SetAssembliesForMappings(List<Assembly> assemblies)
        {
            Config.AssembliesForMapping = assemblies;
        }

    }
}
