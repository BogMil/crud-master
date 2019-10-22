using System;
using System.Linq.Expressions;

namespace CrudMaster.PropertyMapper
{
    public interface IPropertyMapper<TEntity> where TEntity:class
    {
        Expression<Func<TEntity, dynamic>> GetPathInEfForDtoFieldExpression(string dtoFieldName);
    }
}
