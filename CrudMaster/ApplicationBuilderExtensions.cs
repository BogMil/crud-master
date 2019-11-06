using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace CrudMaster
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCrudMaster(this IApplicationBuilder app,Action<CrudMastedConfig> crudMasterConfigs=null)
        {
            Config.AppAssembly = Assembly.GetCallingAssembly();
            var crudMasterConfig = new CrudMastedConfig();
            crudMasterConfigs?.Invoke(crudMasterConfig);
            return app;
        }
    }
}
