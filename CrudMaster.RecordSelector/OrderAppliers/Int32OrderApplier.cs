using System;
using System.Linq;
using System.Linq.Expressions;
using CrudMaster.Utils;

namespace CrudMaster.RecordSelector.OrderAppliers
{
    public class Int32OrderApplier<TEntity> : IOrderApplier<TEntity>
    {
        public IOrderedQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> queryable,
            OrderInstruction orderInstruction)
            => orderInstruction.Direction == OrderDirections.Ascending
                ? queryable.OrderBy((Expression<Func<TEntity, int>>) orderInstruction.LambdaExpression)
                : queryable.OrderByDescending((Expression<Func<TEntity, int>>) orderInstruction.LambdaExpression);

        public IOrderedQueryable<TEntity> ApplyOrdering(IOrderedQueryable<TEntity> queryable,
            OrderInstruction orderInstruction)
            => orderInstruction.Direction == OrderDirections.Ascending 
                ? queryable.ThenBy((Expression<Func<TEntity, int>>) orderInstruction.LambdaExpression)
                : queryable.ThenByDescending((Expression<Func<TEntity, int>>) orderInstruction.LambdaExpression);
    }
}