using AutoMapper;
using crudMasterApi.Models;

namespace crudMasterApi
{
    public class AutoMapperConfiguration 
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration
            (
                cfg =>
                {
                   
                    cfg.AddProfile<CityMappingProfile>();
                    cfg.AddProfile<SchoolMappingProfile>();
                }
            );
            config.AssertConfigurationIsValid();

            return config;
        }
    }
}