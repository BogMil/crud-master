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
            var userDefinedMapExpressions = GetUserDefinedMapExpressions(sourceType, destinationType);
            var res = new List<string>();
            userDefinedMapExpressions.ForEach(s =>
            {

                res.AddRange(new StringifiedExpression(s).GetIncludings());
            });
            var getInc = _GetUserDefinedMapExpressions(sourceType, destinationType);
            return getInc.Distinct();
        }

        private List<LambdaExpression> GetUserDefinedMapExpressions(Type sourceType, Type destinationType)
        {
            var typeMap = GetTypeMapFor(sourceType, destinationType);
            var userDefinedPropertyMaps = typeMap.PropertyMaps
                .Where(propertyMap => propertyMap.CustomMapExpression != null)
                .Select(propertyMap => propertyMap.CustomMapExpression)
                .ToList();

            return userDefinedPropertyMaps;
        }

        private List<string> _GetUserDefinedMapExpressions(Type sourceType, Type destinationType)
        {
            var _including = new List<string>();

            var userDefinedPropertyMaps = 
                GetTypeMapFor(sourceType, destinationType).PropertyMaps
                .Where(propertyMap => propertyMap.CustomMapExpression != null)
                .ToList();

            foreach (var userDefinedPropertyMap in userDefinedPropertyMaps)
            {
                if (!BaseTypes.IsBaseType(userDefinedPropertyMap.DestinationType))
                {
                    var nestedUserDefinedMapExpressions =
                        _GetUserDefinedMapExpressionsWithPrefix(userDefinedPropertyMap.SourceType,
                            userDefinedPropertyMap.DestinationType,"");

                    _including.AddRange(nestedUserDefinedMapExpressions);
                }
                else
                {
                    _including.AddRange(new StringifiedExpression(userDefinedPropertyMap.CustomMapExpression).GetIncludings());

                }
            }

            return _including.Distinct().ToList();
        }

        private List<string> _GetUserDefinedMapExpressionsWithPrefix(Type sourceType, Type destinationType,string prefix)
        {
            var _including = new List<string>();

            var userDefinedPropertyMaps =
                GetTypeMapFor(sourceType, destinationType).PropertyMaps
                    .Where(propertyMap => propertyMap.CustomMapExpression != null)
                    .ToList();

            foreach (var userDefinedPropertyMap in userDefinedPropertyMaps)
            {
                if (!BaseTypes.IsBaseType(userDefinedPropertyMap.DestinationType))
                {
                    var newPrefix = prefix != "" ? prefix + "." + sourceType.Name : sourceType.Name;
                    var nestedUserDefinedMapExpressions =
                        _GetUserDefinedMapExpressionsWithPrefix(userDefinedPropertyMap.SourceType,
                            userDefinedPropertyMap.DestinationType,newPrefix);

                    _including.AddRange(nestedUserDefinedMapExpressions);
                }
                else
                {
                    var x=new StringifiedExpression(userDefinedPropertyMap.CustomMapExpression).GetIncludings();
                    _including.AddRange(x);
                }
            }
            var z= _including.Count>0 
                ? prefix!="" 
                    ?_including.Select(s=>prefix+"."+s).ToList() 
                    : _including.ToList()
                : new List<string>{prefix+"."+sourceType.Name};

            return z.Distinct().ToList();
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
            var propertyMap = GetPropertyMapForDestinationPropertyName(typeMap,destinationPropertyName);
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
            var fkPropertyMap = GetPropertyMapForDestinationPropertyName(typeMap,destinationFkName);
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
