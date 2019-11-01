using System.Reflection;
using AutoMapper;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi
{
    public class AutoMapperConfiguration 
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration
            (
                cfg =>
                {
                    cfg.AddMaps(Assembly.GetExecutingAssembly());
                }
            );
            config.AssertConfigurationIsValid();

            return config;
        }
    }
}