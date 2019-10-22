using CrudMaster.Repository;
using crudMasterApi.Entities;
using crudMasterApi.Models;
using crudMasterApi.Repositories.Interfaces;

namespace crudMasterApi.Repositories
{
    public class SchoolRepository :
            GenericRepository<School, AccountingContext, SchoolOrderByPredicateCreator, SchoolWherePredicateCreator>,
		ISchoolRepository
	{
        public SchoolRepository(AccountingContext context) : base(context)
        {
        }

        public override object GetPrimaryKeyValue(School entity)
		{
			return entity.Id;		
		}
	}
}