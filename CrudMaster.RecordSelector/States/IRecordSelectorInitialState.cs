using CrudMaster.RecordSelector.Operations;

namespace CrudMaster.RecordSelector.States
{
    public interface IRecordSelectorInitialState<TEntity> :
        IPaginate<TEntity>,
        IGetAll<TEntity>,
        IInclude<TEntity>,
        IWhere<TEntity>,
        IApplyOrders<TEntity>
    {

    }
}