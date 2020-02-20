using CrudMaster;
using CrudMaster.Controller;
using CrudMaster.Sorter;
using Microsoft.AspNetCore.Mvc;

namespace CrudMasterApi.School
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : GenericController<ISchoolService, SchoolQueryDto, SchoolCommandDto>
    {
        public SchoolController(ISchoolService service) : base(service) { }

        public override ActionResult Get(Pager pager, OrderByProperties orderByProperties, string filters)
        {
            var x=Service.List();
            return Ok(x);
            //return base.Get(pager, orderByProperties, filters);
        }
    }
}
