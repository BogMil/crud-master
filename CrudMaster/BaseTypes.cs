using System;
using System.Collections.Generic;
using System.Text;

namespace CrudMaster
{
    public class BaseTypes
    {
        private static readonly List<Type> _baseTypes=new List<Type>()
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
            typeof(Int32),
        };

        public static bool IsBaseType(Type type)
        {
            return _baseTypes.Contains(type);
        }
    }
}
