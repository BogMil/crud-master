using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using CrudMaster.Extensions;
using CrudMaster.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudMaster.Controller
{

    public class CrudMasterController<TService, TQueryDto, TCommandDto> 
        : Microsoft.AspNetCore.Mvc.Controller

        where TQueryDto : class
        where TCommandDto : class
        where TService : IGenericService<TQueryDto,TCommandDto>
    {
        protected TService Service;

        public CrudMasterController(TService service)
        {
            Service = service;
        }

        //[HttpGet("[action]")]
        //public virtual ActionResult OptionsForForeignKey(string fkName,string template,string concatenator=" ")
        //{
        //    var data = Service.OptionsForForeignKey(fkName, template,concatenator);
        //    return Ok(data.ToArray());
        //}

        public string LowerizeColumnNamesInTemplate(string template)
        {
            var regex = new Regex(@"\${(.*?)}");
            var matches = regex.Matches(template);

            foreach (Match match in matches)
            {
                template = template.Replace(match.Value, match.Value.ToLower());
            }

            return template;
        }

        [HttpGet]
        public virtual ActionResult Get([FromQuery] Pager pager, [FromQuery] string orderProperties, [FromQuery] string filters)
        {
            try
            {
                return Ok(Service.Get(pager, filters, orderProperties).AsViewModel());
            }
            catch(Exception e)
            {
                return StatusCode(500, ErrorMessageCreator.GetMessageAsString(e));
            }
        }
            

        [HttpPost]
        public virtual ActionResult Post([FromBody] TCommandDto dto)
        {
            try
            {
                Service.Create(dto);
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessageCreator.GetMessageAsString(e));
            }
        }


        [HttpPut]
        public virtual ActionResult Put([FromBody] TCommandDto dto)
        {
            try
            {
                Service.Update(dto);
                return StatusCode(204);
            }
            catch (Exception e)
            {
                return StatusCode(500, ErrorMessageCreator.GetMessageAsString(e));
            }
        }

        [HttpDelete]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                Service.Delete(id);
                return StatusCode(204);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageCreator.GetMessageAsHttpsStatusCode(e));
            }
        }
    }
}