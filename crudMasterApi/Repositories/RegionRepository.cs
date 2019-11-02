using CrudMaster.Repository;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi.Repositories
{
    public interface IRegionRepository : IGenericRepository<Region>
    {

    }
    public class RegionRepository :
        GenericRepository<Region, AccountingContext, RegionOrderByPredicateCreator, RegionWherePredicateCreator>,
        IRegionRepository
    {
        public RegionRepository(AccountingContext context) : base(context)
        {
        }
    }
}