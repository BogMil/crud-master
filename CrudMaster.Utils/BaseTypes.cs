using System;
using System.Collections.Generic;

namespace CrudMaster.Utils
{
    public class BaseTypes
    {
        private static readonly List<Type> BaseTypeCollection=new List<Type>()
        {
            typeof(Int32),
            typeof(Int64),
            typeof(Decimal),
            typeof(String),
            typeof(long),
            typeof(int),
            typeof(bool),
            typeof(double),
            typeof(float),
            typeof(DateTime),
        };

        public static bool IsBaseType(Type type)
        {
            return BaseTypeCollection.Contains(type);
        }
    }
}
