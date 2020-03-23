using CrudMaster.RecordSelector.States;

namespace CrudMaster.RecordSelector.Operations
{
    public interface IApplyOrders<TEntity>
    {
        IApplyOrdersState<TEntity> ApplyOrders();
    }
}