using CrudMaster;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Repositories;

namespace CrudMasterApi.Services.CrudMaster
{
    public interface ICityService : IGenericService<CityQueryDto, CityCommandDto> { }
    public class CityService : GenericService<CityQueryDto,CityCommandDto,ICityRepository,City>,ICityService
	{
		public CityService(ICityRepository repository) : base(repository) { }
    }
}