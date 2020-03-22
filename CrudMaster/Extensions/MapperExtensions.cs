using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace CrudMaster.Extensions
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
}
