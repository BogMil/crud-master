using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

        //public void SetBaseAppAssembly(Assembly assembly) => Config.AppAssembly = assembly;
    }
}
