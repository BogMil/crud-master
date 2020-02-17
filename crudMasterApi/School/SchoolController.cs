using CrudMaster.Controller;
using Microsoft.AspNetCore.Mvc;

namespace CrudMasterApi.School
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : GenericController<ISchoolService, SchoolQueryDto, SchoolCommandDto>
    {
        public SchoolController(ISchoolService service) : base(service) { }
    }
}
