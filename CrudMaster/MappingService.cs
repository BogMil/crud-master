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

        LambdaExpression GetPropertyMappingExpression(string destinationPropertyName,
            Type destinationType, Type sourceType);

        string GetPropertyPathInSourceType(string destinationPropertyName, Type destinationType, Type sourceType);

        string GetFkNameInSourceForDestinationFkName(string destinationFkName, Type destinationType, Type sourceType);
        StaticPagedList<TDestination> MapToStaticPageList<TSource, TDestination>(IPagedList<TSource> source);
        IEnumerable<string> GetIncludings(Type sourceType, Type destinationType);

    }

    public class MappingService : IMappingService
    {
        public IMapper Mapper { get; set; }
        private readonly IncludingsExtractor _includingsExtractor;

        public MappingService()
        {
            Mapper = Mapping.GetMapper();
            _includingsExtractor = new IncludingsExtractor();
        }

        public TDestination Map<TSource, TDestination>(TSource source) 
            => Mapper.Map<TSource, TDestination>(source);

        public StaticPagedList<TDestination> MapToStaticPageList<TSource, TDestination>(IPagedList<TSource> source) 
            => Mapper.Map<IPagedList<TSource>, StaticPagedList<TDestination>>((PagedList<TSource>)source);

        public IEnumerable<string> GetIncludings(Type sourceType, Type destinationType)
        {
            var mapping = Mapper.GetTypeMapFor(sourceType, destinationType);
            return _includingsExtractor.Extract(mapping);
        }

        public TypeMap GetTypeMapFor(Type sourceType, Type destinationType) 
            => Mapper.GetTypeMapFor(sourceType, destinationType);


        public LambdaExpression GetPropertyMappingExpression(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            var typeMap = Mapper.GetTypeMapFor(sourceType, destinationType);

            var propertyMap = typeMap.GetPropertyMapByDestinationPropertyName(destinationPropertyName);
            if (propertyMap.CustomMapExpression != null)
                return propertyMap.CustomMapExpression;

            var sourceTypeLambdaExpressionCreatorType = typeof(LambdaExpressionFromPath<>).MakeGenericType(sourceType);
            dynamic linkedTableExpressionCreator =
                Activator.CreateInstance(sourceTypeLambdaExpressionCreatorType, destinationPropertyName);
            return linkedTableExpressionCreator.LambdaExpression;
        }

        public LambdaExpressionFromPath<dynamic> GetExpressionCreatorForMappingFromDestinationPropToSourceProp(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            //var typeMap = Mapper.GetTypeMapFor(sourceType, destinationType);

            //var propertyMap = typeMap.GetPropertyMapByDestinationPropertyName(destinationPropertyName);
            //if (propertyMap.CustomMapExpression != null)
            //    return propertyMap.CustomMapExpression;

            var sourceTypeLambdaExpressionCreatorType = typeof(LambdaExpressionFromPath<>).MakeGenericType(destinationType);
            dynamic linkedTableExpressionCreator =
                Activator.CreateInstance(sourceTypeLambdaExpressionCreatorType, destinationPropertyName);
            return linkedTableExpressionCreator;
        }

        public string GetPropertyPathInSourceType(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            var mapingExpression =
                GetPropertyMappingExpression(destinationPropertyName, destinationType, sourceType);
            var x = mapingExpression.Compile();

            var expressionBodyString = mapingExpression.Body.ToString();
            var firstDotIndex = expressionBodyString.IndexOf(".") + 1;
            var propertyPath = expressionBodyString.Substring(firstDotIndex, expressionBodyString.Length - firstDotIndex);
            return propertyPath;
        }

        public string GetFkNameInSourceForDestinationFkName(string destinationFkName, Type destinationType, Type sourceType)
        {
            var typeMap = GetTypeMapFor(sourceType, destinationType: destinationType);
            var fkPropertyMap = typeMap.GetPropertyMapByDestinationPropertyName(destinationFkName);
            return fkPropertyMap.GetNameOfForeignKeyInSource();
        }
    }
}
