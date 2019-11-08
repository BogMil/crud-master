using System;
using System.Collections.Generic;
using System.Linq;

namespace CrudMaster.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOfGenericType(this Type toCheck , Type generic, List<Type> typeArguments = null)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    if (typeArguments != null)
                        return typeArguments.Intersect(cur.GenericTypeArguments).Count() == typeArguments.Count;
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
