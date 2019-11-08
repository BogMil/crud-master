using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrudMaster.Service;

namespace CrudMaster.Extensions
{
    public static class AssemblyExtensions
    {
        public static List<Type> GetTypesThatImplementsGenericInterface(this Assembly assembly, Type genericInterface, List<Type> genericTypeArguments)
        {
            return
                (from type in assembly.GetTypes().ToList()
                 let implementsInterface = type.GetInterfaces()
                     .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface && (genericTypeArguments.Intersect(i.GenericTypeArguments)).Count() == genericTypeArguments.Count)
                 where implementsInterface
                 select type).ToList();
        }

        public static List<Type> GetTypesThatImplementsGenericType(this Assembly assembly, Type genericType, List<Type> genericTypeArguments=null)
        {
            var types = assembly.GetTypes()
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.IsSubclassOfGenericType(genericType,genericTypeArguments)).ToList();

            return types;
        }

        public static List<Type> GetCrudMasterServicesThatHaveTypeArguments(this Assembly assembly,List<Type> genericTypeArguments = null)
        {
            var services =
                assembly.GetTypesThatImplementsGenericType(typeof(GenericService<,,,>), genericTypeArguments);

            return services;
        }

        public static Type GetCrudMasterServiceWithTEntity(this Assembly assembly, Type TEntity)
        {
            var types = assembly.GetTypes()
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.IsSubclassOfGenericType(typeof(GenericService<,,,>)))
                .Where(t => t.BaseType != null && t.BaseType.GenericTypeArguments.Last() == TEntity)
                .ToList();
            if(types.Count>1)
                throw new Exception("Found more than one Crud Master Service for same entity");
            if (types.Count == 0)
                throw new Exception($"No Crud Master Service found for entity:{TEntity}");

            return types.First();
        }
    }
}
