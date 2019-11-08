using AutoMapper;
using CrudMaster.Repository;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi.Repositories
{
    public interface ISchoolRepository : IGenericRepository<School> { }

    public class SchoolRepository :
            GenericRepository<School, AccountingContext>,
		ISchoolRepository
	{
        public SchoolRepository(AccountingContext context,IMapper mapper) : base(context,mapper) { }
    }
}