using CrudMaster.RecordSelector.Operations;

namespace CrudMaster.RecordSelector.States
{
    public interface IWhereState<TEntity> :
        IPaginate<TEntity>,IGetAll<TEntity>,
        IApplyOrders<TEntity>
    { }
}