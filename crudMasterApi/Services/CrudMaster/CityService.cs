using AutoMapper;
using CrudMaster.Service;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Repositories;

namespace CrudMasterApi.Services.CrudMaster
{
    public interface ICityService : IGenericService<CityQueryDto, CityCommandDto>
    {

    }
    public class CityService : GenericService<CityQueryDto,CityCommandDto,ICityRepository,City>,ICityService
	{
		public CityService(ICityRepository repository, IMapper mapper) : base(repository, mapper)
        {

        }
	}
}