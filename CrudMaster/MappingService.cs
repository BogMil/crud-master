using AutoMapper;
using CrudMaster.Extensions;
using CrudMaster.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        {
            var userDefinedPropertyMaps =
                GetUserDefinedPropertyMaps(sourceType, destinationType);

            var includings = new List<string>();

            foreach (var pm in userDefinedPropertyMaps)
            {
                var pmIncludings = GetIncludingsForPropertyMap(pm);
                includings.AddRange(pmIncludings);
            }

            return includings.Distinct().ToList();
        }
        
        private List<string> GetIncludingsWithPrefix(Type sourceType, Type destinationType, string prefix = "")
        {
            var userDefinedPropertyMaps = GetUserDefinedPropertyMaps(sourceType,destinationType);
            var includings = new List<string>();
            foreach (var pm in userDefinedPropertyMaps)
            {
                var newPrefix = prefix != "" ? prefix + "." + sourceType.Name : sourceType.Name;
                var pmIncludings= GetIncludingsForPropertyMap(pm, newPrefix);
                includings.AddRange(pmIncludings);
            }
            var z = includings.Count > 0
                ? prefix != ""
                    ? includings.Select(s => prefix + "." + s).ToList()
                    : includings.ToList()
                : new List<string> { prefix + "." + sourceType.Name };

            return z.Distinct().ToList();
        }

        private List<PropertyMap> GetUserDefinedPropertyMaps(Type sourceType, Type destinationType)
        {
            return GetTypeMapFor(sourceType, destinationType).PropertyMaps
                .Where(propertyMap => propertyMap.CustomMapExpression != null)
                .ToList();
        }

        private List<string> GetIncludingsForPropertyMap(PropertyMap pm,string prefix="")
        {
            if (!BaseTypes.IsBaseType(pm.DestinationType))
                return GetIncludingsWithPrefix(pm.SourceType, pm.DestinationType, prefix);
            else
                return new StringifiedExpression(pm.CustomMapExpression).GetIncludings().ToList();
        }

        public TypeMap GetTypeMapFor(Type sourceType, Type destinationType)
        {
            var mappings = Mapper.ConfigurationProvider.GetAllTypeMaps()
                .Where(s => s.SourceType == sourceType && s.DestinationType == destinationType).ToList();

            if (mappings.Count == 0)
                throw new Exception($"Mappings not found for source:{sourceType} and destination:{destinationType}");
            if (mappings.Count > 1)
                throw new Exception($"Multiple mappings found for source:{sourceType} and destination:{destinationType}");

            return mappings.First();
        }

        public LambdaExpression GetPropertyMapExpression(string destinationPropertyName, Type destinationType, Type sourceType)
        {
            var typeMap = GetTypeMapFor(sourceType, destinationType);
            var propertyMap = GetPropertyMapForDestinationPropertyName(typeMap, destinationPropertyName);
            return GetSourceLambdaExpression(propertyMap);
        }


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
            var fkPropertyMap = GetPropertyMapForDestinationPropertyName(typeMap, destinationFkName);
            return fkPropertyMap.GetNameOfForeignKeyInSource();
        }

        private PropertyMap GetPropertyMapForDestinationPropertyName(TypeMap typeMap, string destinationPropertyNameName)
        {
            var propertyMapsForDestionationName = typeMap.PropertyMaps.Where(map => map.DestinationName == destinationPropertyNameName).ToList();
            if (propertyMapsForDestionationName.Count == 0)
                throw new Exception($"No mapping found for destinationName:{destinationPropertyNameName}");
            if (propertyMapsForDestionationName.Count > 1)
                throw new Exception($"Multiple mappings found for destinationName:{destinationPropertyNameName}");

            return propertyMapsForDestionationName.First();
        }

        private LambdaExpression GetSourceLambdaExpression(PropertyMap propertyMap)
        {
            if (propertyMap.CustomMapExpression != null)
                return propertyMap.CustomMapExpression;

            return LambdaExpressionFromPathFactory
                .CreateInRuntime(propertyMap.TypeMap.SourceType, propertyMap.DestinationName)
                .LambdaExpression;
        }
    }

}
