using CrudMaster.Controller;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Services.CrudMaster;
using Microsoft.AspNetCore.Mvc;

namespace CrudMasterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : GenericController<IModuleService, ModuleQueryDto, ModuleCommandDto>
    {
        public ModuleController(IModuleService service) : base(service) { }
    }
}