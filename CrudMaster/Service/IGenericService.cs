using System.Collections.Generic;
using CrudMaster.Sorter;
using X.PagedList;

namespace CrudMaster.Service
{
    public interface IGenericService<TQueryDto, TCommandDto>
    {
        void Create(TCommandDto dto);
        TQueryDto CreateAndReturn(TCommandDto dto);
        TModel CreateAndReturnModel<TModel>(TCommandDto dto);


        void Update(TCommandDto dto);
        TQueryDto UpdateAndReturn(TCommandDto dto);
        TModel UpdateAndReturnModel<TModel>(TCommandDto dto);


        void Delete(int id);

        int DeleteAndReturn(int id);
        TQueryDto Find(int id);
        StaticPagedList<TQueryDto> GetJqGridDataTest(Pager pager, string filters, OrderByProperties orderByProperties);
        Dictionary<string, string> OptionsForForeignKey(string fkName, string templateWithColumnNames, string concatenator);


    }
}