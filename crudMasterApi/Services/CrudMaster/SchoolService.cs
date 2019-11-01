using AutoMapper;
using CrudMaster.Service;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Repositories;

namespace CrudMasterApi.Services.CrudMaster
{
    public interface ISchoolService : IGenericService<SchoolQueryDto, SchoolCommandDto>
    {

    }

    public class SchoolService : GenericService<SchoolQueryDto,SchoolCommandDto,ISchoolRepository,School>,ISchoolService
	{
		public SchoolService(ISchoolRepository repository, IMapper mapper) : base(repository, mapper)
        {

        }
	}
}