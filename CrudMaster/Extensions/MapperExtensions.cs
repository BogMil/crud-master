using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;

namespace CrudMaster
{
    public static class MapperExtensions
    {
        public static TypeMap GetTypeMapFor(this IMapper mapper, Type sourceType, Type destinationType)
        {
            var mappings = mapper.ConfigurationProvider.GetAllTypeMaps()
                .Where(s => s.SourceType == sourceType && s.DestinationType == destinationType).ToList();

            if (mappings.Count == 0)
                throw new Exception($"Mappings not found for source:{sourceType} and destination:{destinationType}");
            if (mappings.Count > 1)
                throw new Exception($"Multiple mappings found for source:{sourceType} and destination:{destinationType}");

            return mappings.First();
        }

        public static List<TypeMap> GetTypeMapsFor(this IMapper mapper, Type sourceType)
        {
            var mappings = mapper.ConfigurationProvider.GetAllTypeMaps()
                .Where(s => s.SourceType == sourceType).ToList();

            if (mappings.Count == 0)
                throw new Exception($"Mappings not found for source:{sourceType}");

            return mappings;
        }
    }

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
    }

    public static class PropertyMapExtensions
    {
        public static string GetNameOfForeignKeyInSource(this PropertyMap propertyMap)
        {
            if (propertyMap.CustomMapExpression == null)
                return propertyMap.DestinationName;

            //In case of foreign key. Fot type T foreign key is always T.FK. It can not be something like T.Somethin.FK
            var expression = propertyMap.CustomMapExpression;
            var fkName = (expression.Body as MemberExpression)?.Member.Name;
            if(fkName.Contains("."))
                throw new Exception("Not foreign key");

            return fkName;

        }
    }

    
}
