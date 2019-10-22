using AutoMapper;
using CrudMaster.Service;
using crudMasterApi.Entities;
using crudMasterApi.Models;
using crudMasterApi.Repositories.Interfaces;
using crudMasterApi.Services.CRUD.Interfaces;

namespace crudMasterApi.Services.CRUD
{
	public class SchoolService : GenericService<SchoolQueryDto,SchoolCommandDto,ISchoolRepository,School>,ISchoolService
	{
		public SchoolService(ISchoolRepository repository, IMapper mapper) : base(repository, mapper)
        {

        }
	}
}