using CrudMaster.Service;
using CrudMasterApi.Entities;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Repositories;

namespace CrudMasterApi.Services.CrudMaster
{
    public interface IModuleService : IGenericService<ModuleQueryDto, ModuleCommandDto> { }
    public class ModuleService : GenericService<ModuleQueryDto, ModuleCommandDto, IModuleRepository, Module>, IModuleService
    {
        public ModuleService(IModuleRepository repository) : base(repository) { }
    }
}