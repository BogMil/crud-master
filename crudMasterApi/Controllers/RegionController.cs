using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudMaster.Controller;
using CrudMasterApi.Models.CrudMaster;
using CrudMasterApi.Services.CrudMaster;
using Microsoft.AspNetCore.Mvc;

namespace CrudMasterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController: GenericController<IRegionService, RegionQueryDto, RegionCommandDto>
    {
        public RegionController(IRegionService service) : base(service) {}
    }
}
