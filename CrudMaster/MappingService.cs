using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CrudMaster.Extensions;
using X.PagedList;

namespace CrudMaster
{
    public interface IMappingService
    {
        TDestination Map<TSource, TDestination>(TSource source);
        IMapper Mapper { get; set; }
        TypeMap GetTypeMapFor(Type sourceType, Type destinationType);

        LambdaExpression GetPropertyMapExpression(string destinationPropertyName,
            Type destinationType, Type sourceType);

        string GetPropertyPathInSourceType(string destinationPropertyName, Type destinationType, Type sourceType);

        string GetFkNameInSourceForDestinationFkName(string destinationFkName, Type destinationType, Type sourceType);
        StaticPagedList<TDestination> MapToStaticPageList<TSource, TDestination>(IPagedList<TSource> source);
        IEnumerable<string> GetIncludings(Type sourceType, Type destinationType);

    }

    public class MappingService : IMappingService
    {
        public IMapper Mapper { get; set; }

        public MappingService()
        {
            Mapper = Mapping.GetMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            => Mapper.Map<TSource, TDestination>(source);

        public StaticPagedList<TDestination> MapToStaticPageList<TSource, TDestination>(IPagedList<TSource> source)
            => Mapper.Map<IPagedList<TSource>, StaticPagedList<TDestination>>((PagedList<TSource>)source);

        public IEnumerable<string> GetIncludings(Type sourceType, Type destinationType)
            => Mapper.GetTypeMapFor(sourceType, destinationType).Extract();

        public TypeMap GetTypeMapFor(Type sourceType, Type destinationType)
            => Mapper.GetTypeMapFor(sourceType, destinationType);

        public LambdaExpression GetPropertyMapExpression(string destinationPropertyName, Type destinationType, Type sourceType)
            => Mapper.GetTypeMapFor(sourceType, destinationType)
                .GetPropertyMapForDestinationPropertyName(destinationPropertyName)
                .GetSourceLambdaExpression();

        public string GetPropertyPathInSourceType(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            var mapingExpression =
                GetPropertyMapExpression(destinationPropertyName, destinationType, sourceType);
            var x = mapingExpression.Compile();

            var expressionBodyString = mapingExpression.Body.ToString();
            var firstDotIndex = expressionBodyString.IndexOf(".") + 1;
            var propertyPath = expressionBodyString.Substring(firstDotIndex, expressionBodyString.Length - firstDotIndex);
            return propertyPath;
        }

        public string GetFkNameInSourceForDestinationFkName(string destinationFkName, Type destinationType, Type sourceType)
        {
            var typeMap = GetTypeMapFor(sourceType, destinationType: destinationType);
            var fkPropertyMap = typeMap.GetPropertyMapForDestinationPropertyName(destinationFkName);
            return fkPropertyMap.GetNameOfForeignKeyInSource();
        }
    }
}
