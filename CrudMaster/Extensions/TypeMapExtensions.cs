using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace CrudMaster.Extensions
{
    public static class TypeMapExtensions
    {
        public static PropertyMap GetPropertyMapForDestinationPropertyName(this TypeMap typeMap, string destinationPropertyNameName)
        {
            var propertyMapsForDestionationName= typeMap.PropertyMaps.Where(map => map.DestinationName == destinationPropertyNameName).ToList();
            if(propertyMapsForDestionationName.Count==0)
                throw new Exception($"No mapping found for destinationName:{destinationPropertyNameName}");
            if (propertyMapsForDestionationName.Count > 1)
                throw new Exception($"Multiple mappings found for destinationName:{destinationPropertyNameName}");

            return propertyMapsForDestionationName.First();
        }

        public static List<LambdaExpression> GetCustomMapExpressions(this TypeMap typeMap)=>
            typeMap.PropertyMaps
                .Where(propertyMap => propertyMap.CustomMapExpression != null)
                .Select(propertyMap => propertyMap.CustomMapExpression)
                .ToList();

        public static IEnumerable<string> Extract(this TypeMap typeMap)
        {
            var res = new List<string>();

            typeMap
                .GetCustomMapExpressions()
                .ForEach(s => res.AddRange(new StringifiedExpression(s).GetIncludings()));
            return res.Distinct();
        }
    }
}