using CrudMaster.RecordSelector.Operations;

namespace CrudMaster.RecordSelector.States
{
    public interface IApplyOrdersState<TEntity>: 
        IGetAll<TEntity>,
        IPaginate<TEntity>
    { }
}