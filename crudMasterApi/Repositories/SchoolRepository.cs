using CrudMaster.Repository;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi.Repositories
{
    public interface ISchoolRepository : IGenericRepository<School> { }

    public class SchoolRepository :
            GenericRepository<School, AccountingContext, SchoolOrderByPredicateCreator, SchoolWherePredicateCreator>,
		ISchoolRepository
	{
        public SchoolRepository(AccountingContext context) : base(context) { }
    }
}