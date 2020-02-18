using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CrudMaster.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static Type GetPropertyType(this PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;
            if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
                propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);

            return propertyType;
        }

        public static List<PropertyInfo> GetPropertiesThatAreNotBaseTypes(this List<PropertyInfo> properties)
        {
            var propertiesThatAreNotBaseTypes = new List<PropertyInfo>();
            properties.ForEach(propertyInfo =>
            {
                var propertyType = propertyInfo.GetPropertyType();
                if (!BaseTypes.IsBaseType(propertyType))
                    propertiesThatAreNotBaseTypes.Add(propertyInfo);
            });

            return propertiesThatAreNotBaseTypes;
        }
    }
}
