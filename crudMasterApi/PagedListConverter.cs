//using System.Collections.Generic;
//using AutoMapper;
//using X.PagedList;

//namespace CrudMasterApi
//{
//    public class PagedListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, StaticPagedList<TDestination>>
//    {
//        private IMapper _mapper { get; set; }

//        public PagedListConverter()
//        {
//            _mapper = new AutoMapperConfiguration().Configure();
//        }

//        StaticPagedList<TDestination> ITypeConverter<PagedList<TSource>, StaticPagedList<TDestination>>.
//            Convert(PagedList<TSource> source, StaticPagedList<TDestination> destination, ResolutionContext context)
//        {
//            var collection = _mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
//            return new StaticPagedList<TDestination>(collection, source.PageNumber, source.PageSize, source.TotalItemCount);
//        }
//    }
//}