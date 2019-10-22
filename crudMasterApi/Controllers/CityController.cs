using CrudMaster.Controller;
using crudMasterApi.Models;
using crudMasterApi.Services.CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace crudMasterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController :
            GenericController<ICityService, CityViewModel, CityQueryDto, CityCommandDto>
    {
        public CityController(ICityService service) : base(service)
        {
        }
    }
}
