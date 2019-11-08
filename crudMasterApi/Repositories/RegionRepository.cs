using AutoMapper;
using CrudMaster.Repository;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi.Repositories
{
    public interface IRegionRepository : IGenericRepository<Region>
    {

    }
    public class RegionRepository : GenericRepository<Region, AccountingContext>, IRegionRepository
    {
        public RegionRepository(AccountingContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}