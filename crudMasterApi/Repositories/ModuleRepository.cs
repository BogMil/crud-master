using CrudMaster.Repository;
using CrudMasterApi.Entities;

namespace CrudMasterApi.Repositories
{
    public interface IModuleRepository : IGenericRepository<Module>
    {

    }
    public class ModuleRepository : GenericRepository<Module, AccountingContext>, IModuleRepository
    {
        public ModuleRepository(AccountingContext context) : base(context) { }
    }
}