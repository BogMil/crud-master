using System.Linq;
using CrudMaster.Utils;

namespace CrudMaster.RecordSelector.OrderAppliers
{
    public interface IOrderApplier<TEntity>
    {
        IOrderedQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> queryable, OrderInstruction orderInstruction);

        IOrderedQueryable<TEntity> ApplyOrdering(IOrderedQueryable<TEntity> queryable,
            OrderInstruction orderInstruction);

    }
}