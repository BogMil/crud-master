using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CrudMaster
{
    public static class AssemblyExtensions
    {
        public static List<Type> GetTypesThatImplements(this Assembly assembly,Type genericInterface, List<Type> genericTypeArguments)
        {
            return
                (from type in assembly.GetTypes().ToList()
                let implementsInterface = type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface && (genericTypeArguments.Intersect(i.GenericTypeArguments)).Count()==genericTypeArguments.Count)
                where implementsInterface
                select type).ToList();
        }
    }
}
