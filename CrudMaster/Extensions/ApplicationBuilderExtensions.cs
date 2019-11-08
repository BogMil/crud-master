using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace CrudMaster.Extensions
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
