using CrudMaster.Repository;
using crudMasterApi.Entities;
using crudMasterApi.Models;
using crudMasterApi.Repositories.Interfaces;

namespace crudMasterApi.Repositories
{
	public class CityRepository : 
			GenericRepository<City,AccountingContext,CityOrderByPredicateCreator,CityWherePredicateCreator>,
		ICityRepository
	{
	    public CityRepository(AccountingContext context) : base(context)
	    {
	    }

	 //   public override object GetPrimaryKey(City entity)
		//{
		//	return entity.Id;		
		//}

	}
}