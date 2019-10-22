using AutoMapper;
using CrudMaster.Service;
using crudMasterApi.Entities;
using crudMasterApi.Models;
using crudMasterApi.Repositories.Interfaces;
using crudMasterApi.Services.CRUD.Interfaces;

namespace crudMasterApi.Services.CRUD
{
	public class CityService : GenericService<CityQueryDto,CityCommandDto,ICityRepository,City>,ICityService
	{
		public CityService(ICityRepository repository, IMapper mapper) : base(repository, mapper)
        {

        }
	}
}