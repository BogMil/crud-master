using AutoMapper;
using CrudMaster.Service;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Repositories;

namespace CrudMasterApi.Services.CrudMaster
{
    public interface IRegionService : IGenericService<RegionQueryDto, RegionCommandDto>
    {

    }
    public class RegionService : GenericService<RegionQueryDto, RegionCommandDto, IRegionRepository, Region>, IRegionService
    {
        public RegionService(IRegionRepository repository) : base(repository)
        {

        }
    }
}