using CrudMaster.Controller;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Services.CrudMaster;
using Microsoft.AspNetCore.Mvc;

namespace CrudMasterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController :
            GenericController<ISchoolService, SchoolViewModel, SchoolQueryDto, SchoolCommandDto>
    {
        public SchoolController(ISchoolService service) : base(service) { }
    }
}
