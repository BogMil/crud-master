using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
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
        List<string> GetIncludings(Type sourceType, Type destinationType);

    }

    public class MappingService : IMappingService
    {
        public IMapper Mapper { get; set; } = Mapping.Mapper;

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public StaticPagedList<TDestination> MapToStaticPageList<TSource, TDestination>(IPagedList<TSource> source)
        {
            return Mapper.Map<IPagedList<TSource>, StaticPagedList<TDestination>>((PagedList<TSource>)source);
        }

        public List<string> GetIncludings(Type sourceType, Type destinationType)
        {
            var mapping = Mapper.GetTypeMapFor(sourceType, destinationType);
            var list = new List<string>();
            foreach (var propertyMap in mapping.PropertyMaps)
            {
                if (propertyMap.CustomMapExpression == null) continue;

                var parameterName = propertyMap.CustomMapExpression.Parameters[0].Name;
                var returnType = propertyMap.CustomMapExpression.ReturnType;
                var str = propertyMap.CustomMapExpression.ToString();
                list.Add(str);
            }

            return null;
        }

        public TypeMap GetTypeMapFor(Type sourceType, Type destinationType)
        {
            return Mapper.GetTypeMapFor(sourceType, destinationType);
        }


        public LambdaExpression GetPropertyMappingExpression(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            var typeMap = Mapper.GetTypeMapFor(sourceType, destinationType);

            var propertyMap = typeMap.GetPropertyMapByDestinationPropertyName(destinationPropertyName);
            if (propertyMap.CustomMapExpression != null)
                return propertyMap.CustomMapExpression;

            var sourceTypeLambdaExpressionCreatorType = typeof(LambdaExpressionCreator<>).MakeGenericType(sourceType);
            dynamic linkedTableExpressionCreator =
                Activator.CreateInstance(sourceTypeLambdaExpressionCreatorType, destinationPropertyName);
            return linkedTableExpressionCreator.LambdaExpression;
        }

        public LambdaExpressionCreator<dynamic> GetExpressionCreatorForMappingFromDestinationPropToSourceProp(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            //var typeMap = Mapper.GetTypeMapFor(sourceType, destinationType);

            //var propertyMap = typeMap.GetPropertyMapByDestinationPropertyName(destinationPropertyName);
            //if (propertyMap.CustomMapExpression != null)
            //    return propertyMap.CustomMapExpression;

            var sourceTypeLambdaExpressionCreatorType = typeof(LambdaExpressionCreator<>).MakeGenericType(destinationType);
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
