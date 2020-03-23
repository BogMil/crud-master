using CrudMaster.RecordSelector.Operations;

namespace CrudMaster.RecordSelector.States
{
    public interface IIncludeState<TEntity> :
        IPaginate<TEntity>,
        IGetAll<TEntity>,
        IWhere<TEntity>,
        IApplyOrders<TEntity>
    { }
}