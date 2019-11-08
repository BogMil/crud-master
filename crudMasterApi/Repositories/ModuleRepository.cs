using AutoMapper;
using CrudMaster.Repository;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;

namespace CrudMasterApi.Repositories
{
    public interface IModuleRepository : IGenericRepository<Module>
    {

    }
    public class ModuleRepository :
        GenericRepository<Module, AccountingContext>,
        IModuleRepository
    {
        public ModuleRepository(AccountingContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}