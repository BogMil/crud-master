using System.Collections.Generic;
using System.Linq;
using CrudMaster.Utils;
using X.PagedList;

namespace CrudMaster.RecordSelector.Operations
{
    public interface IPaginate<TEntity>
    {
        IPagedList<TEntity> Paginate(Pager pager);
    }
}