using CrudMaster.Controller;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Services.CrudMaster;
using Microsoft.AspNetCore.Mvc;

namespace CrudMasterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController :
            GenericController<ICityService, CityQueryDto, CityCommandDto>
    {
        public CityController(ICityService service) : base(service) {}
    }
}
