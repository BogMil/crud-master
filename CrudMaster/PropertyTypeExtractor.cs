using System;
using System.Reflection;

namespace CrudMaster
{
    public class PropertyTypeExtractor<TEntity>
    {
        public static string GetPropertyTypeName(string propertyPath)
        {
            var lastAccessedPropertyInfo = GetPropertyInfo(propertyPath);
            var lastAccessedPropertyType = lastAccessedPropertyInfo.PropertyType;

            return lastAccessedPropertyType.Name.Contains("Nullable") ?
                "Nullable." + lastAccessedPropertyType.GenericTypeArguments[0].Name
                :
                lastAccessedPropertyType.Name;

        }

        public static PropertyInfo GetPropertyInfo( string propertyPath)
        {
            var type = typeof(TEntity);
            while (propertyPath.Contains("."))
            {
                var propertyPathSplitedOnFirstDot = propertyPath.Split(new[] { '.' }, 2);
                var accessedProperty = type.GetProperty(propertyPathSplitedOnFirstDot[0]);

                if (accessedProperty == null)
                    throw new MissingMemberException();

                propertyPath = propertyPathSplitedOnFirstDot[1];
                type = accessedProperty.PropertyType;
            }

            var pi = type.GetProperty(propertyPath);
            if (pi == null)
                throw new NullReferenceException();

            return pi;
        }


    }
}