using CrudMaster;
using CrudMasterApi.Entities;

namespace CrudMasterApi.School
{
    public interface ISchoolRepository : IGenericRepository<Entities.School> { }

    public class SchoolRepository : GenericRepository<Entities.School, AccountingContext>, ISchoolRepository
	{
        public SchoolRepository(AccountingContext context) : base(context) { }
    }
}