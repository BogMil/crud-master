using System;
using System.Linq;
using System.Text.RegularExpressions;
using CrudMaster.Service;
using CrudMaster.Sorter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudMaster.Controller
{

    public class GenericController<TService, TQueryDto, TCommandDto> 
        : Microsoft.AspNetCore.Mvc.Controller

        where TQueryDto : class
        where TCommandDto : class
        where TService : IGenericService<TQueryDto,TCommandDto>
    {
        protected TService Service;

        public GenericController(TService service)
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
        public virtual ActionResult Get([FromQuery] Pager pager, [FromQuery] OrderByProperties orderByProperties, [FromQuery] string filters)
        {
            var data = Service.GetJqGridDataTest(pager, filters, orderByProperties);
            var viewModel = new GenericViewModel<TQueryDto>
            {
                CurrentPageNumber = data.PageNumber,
                TotalNumberOfPages = data.PageCount,
                TotalNumberOfRecords = data.TotalItemCount,
                Records = data.ToList()
            };

            return Ok(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Post([FromBody] TCommandDto dto)
        {
            try
            {
                var createdDto = Service.CreateAndReturn(dto);
                return Json(createdDto);
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
                var createdDto = Service.UpdateAndReturn(dto);
                return Json(createdDto);
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
                var deletedId = Service.DeleteAndReturn(id);
                return Json(deletedId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageCreator.GetMessageAsHttpsStatusCode(e));
            }
        }
        //public virtual ActionResult AjaxJqGrid(Pager pager,OrderByProperties orderByProperties,string filters)
        //{
        //    var data = Service.GetJqGridData(pager, filters,orderByProperties);
        //    var jqGridViewModal = JqGridViewModelFactory(data);
        //    return Json(jqGridViewModal);
        //}


        //public static TReturnViewModel JqGridViewModelFactory<TReturnViewModel, QueryDto>(StaticPagedList<QueryDto> listOfDto) 
        //    where TReturnViewModel:GenericJqGridViewModel<QueryDto>,new()
        //    where QueryDto :class
        //{
        //    var viewModel = new TReturnViewModel
        //    {
        //        Records = listOfDto.ToList(),
        //        CurrentPageNumber = listOfDto.PageNumber,
        //        TotalNumberOfPages = listOfDto.PageCount,
        //        TotalNumberOfRecords = listOfDto.TotalItemCount,
        //    };

        //    return viewModel;
        //}


        //[HttpPost]
        //public virtual ActionResult Create(TCommandDto dto)
        //{
        //    try
        //    {
        //        var createdDto = Service.CreateAndReturn(dto);
        //        return Json(createdDto);
        //    }
        //    catch (Exception e)
        //    {
        //        //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ErrorMessageCreator.GetMessageAsString(e));
        //        return StatusCode(500, ErrorMessageCreator.GetMessageAsString(e));
        //    }
        //}

        //[HttpPost]
        //public virtual ActionResult Update(TCommandDto dto)
        //{
        //    try
        //    {
        //        var updatedDto = Service.UpdateAndReturn(dto);
        //        return Json(updatedDto);
        //    }
        //    catch (Exception e)
        //    {

        //        return StatusCode(500, ErrorMessageCreator.GetMessageAsString(e));

        //    }
        //}

        //[HttpPost]
        //protected virtual ActionResult CreateAndReturnModel<TModel>(TCommandDto dto)
        //{
        //    try
        //    {
        //        var createdDto = Service.CreateAndReturnModel<TModel>(dto);
        //        return Json(createdDto);
        //    }
        //    catch (Exception e)
        //    {
        //        //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ErrorMessageCreator.GetMessageAsHttpsStatusCode(e));
        //        return StatusCode(500, ErrorMessageCreator.GetMessageAsHttpsStatusCode(e));

        //    }
        //}

        //[HttpPost]
        //protected virtual ActionResult UpdateAndReturnModel<TModel>(TCommandDto dto)
        //{
        //    try
        //    {
        //        var createdDto = Service.UpdateAndReturnModel<TModel>(dto);
        //        return Json(createdDto);
        //    }
        //    catch (Exception e)
        //    {
        //        //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ErrorMessageCreator.GetMessageAsHttpsStatusCode(e));
        //        return StatusCode(500, ErrorMessageCreator.GetMessageAsHttpsStatusCode(e));
        //    }
        //}

        //[HttpPost]
        //public virtual ActionResult DeleteAndReturn(int id)
        //{
        //    try
        //    {
        //        var deletedId = Service.DeleteAndReturn(id);
        //        return Json(deletedId);
        //    }
        //    catch (Exception e)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageCreator.GetMessageAsHttpsStatusCode(e));
        //    }
        //}
    }
}