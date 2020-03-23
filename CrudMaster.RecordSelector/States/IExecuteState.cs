using CrudMaster.RecordSelector.Operations;

namespace CrudMaster.RecordSelector.States
{
    public interface IExecuteState<TEntity> :
        IPaginate<TEntity>,
        IGetAll<TEntity> { }
}