// ReSharper disable InconsistentNaming

using ExpressionBuilder.Generics;

namespace CrudMaster.Filter
{
    public interface IWherePredicateCreator<TEntity> where TEntity : class
    {
        Filter<TEntity> GetWherePredicate(string filters);
    }
}