using CrudMaster.Service;

namespace CrudMasterApi.School
{
    public interface ISchoolService : IGenericService<SchoolQueryDto, SchoolCommandDto> { }

    public class SchoolService : GenericService<SchoolQueryDto,SchoolCommandDto,ISchoolRepository,Entities.School>,ISchoolService
	{
		public SchoolService(ISchoolRepository repository) : base(repository) { }
	}
}