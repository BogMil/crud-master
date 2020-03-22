using System;
using System.Linq.Expressions;
using AutoMapper;

namespace CrudMaster.Extensions
{
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