using CrudMaster;
using CrudMasterApi.Entities;

namespace CrudMasterApi.Repositories
{
    public interface ICityRepository : IGenericRepository<City> { }
    public class CityRepository : GenericRepository<City, AccountingContext>, ICityRepository
    {
        public CityRepository(AccountingContext context) : base(context)
        {
        }
    }
}