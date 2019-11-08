using System;
using System.Linq.Expressions;
using CrudMaster.Extensions;

namespace CrudMaster.PropertyMapper
{
    public abstract class GenericPropertyMapper<TEntity,TQueryDto> : IPropertyMapper<TEntity> 
        where TEntity : class
    {
        public abstract Expression<Func<TEntity, dynamic>> GetCorespondingPropertyNavigationInEntityForDtoField(string dtoFieldName);

        /// <summary>
        /// Function returns expresion body withoud expression parameter as string
        /// </summary>
        /// <param name="exp"></param>
        /// <returns>string</returns>
        public string GetExpressionBodyWithoutParameter(Expression<Func<TQueryDto, dynamic>> exp) => exp.GetExpressionBodyAsString();
        public string GetExpressionBodyWithoutParameterToLower(Expression<Func<TQueryDto, dynamic>> exp) => exp.GetExpressionBodyAsString().ToLower();

    }
}
