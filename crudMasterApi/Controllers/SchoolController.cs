using CrudMaster.Controller;
using crudMasterApi.Models;
using crudMasterApi.Services.CRUD.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace crudMasterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController :
            GenericController<ISchoolService, SchoolViewModel, SchoolQueryDto, SchoolCommandDto>
    {
        public SchoolController(ISchoolService service) : base(service)
        {

        }
    }
}
