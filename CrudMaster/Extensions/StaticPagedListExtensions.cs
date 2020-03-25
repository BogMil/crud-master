using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X.PagedList;

namespace CrudMaster.Extensions
{
    public static class StaticPagedListExtensions
    {
        public static GenericViewModel<T> AsViewModel<T>(this StaticPagedList<T> staticPagedList)
            => new GenericViewModel<T>
            {
                CurrentPageNumber = staticPagedList.PageNumber,
                TotalNumberOfPages = staticPagedList.PageCount,
                TotalNumberOfRecords = staticPagedList.TotalItemCount,
                Records = staticPagedList.ToList()
            };
    }
}
