using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace CrudMaster.Extensions
{
    public static class TypeMapExtensions
    {
        public static PropertyMap GetPropertyMapByDestinationPropertyName(this TypeMap typeMap, string destinationPropertyNameName)
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
    }
}