using System.Collections.Generic;
using AutoMapper;
using X.PagedList;

namespace CrudMaster
{
    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, StaticPagedList<TDestination>>
    {
        
        StaticPagedList<TDestination> ITypeConverter<PagedList<TSource>, StaticPagedList<TDestination>>.
            Convert(PagedList<TSource> source, StaticPagedList<TDestination> destination, ResolutionContext context)
        {
            var collection = Mapping.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
            return new StaticPagedList<TDestination>(collection, source.PageNumber, source.PageSize, source.TotalItemCount);
        }
    }
}