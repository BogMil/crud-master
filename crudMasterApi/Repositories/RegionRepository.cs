using CrudMaster;
using CrudMasterApi.Entities;

namespace CrudMasterApi.Repositories
{
    public interface IRegionRepository : IGenericRepository<Region>
    {

    }
    public class RegionRepository : GenericRepository<Region, AccountingContext>, IRegionRepository
    {
        public RegionRepository(AccountingContext context) : base(context)
        {
        }
    }
}