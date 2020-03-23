using X.PagedList;

namespace CrudMaster.RecordSelector.Operations
{
    public interface IGetAll<TEntity>
    {
        IPagedList<TEntity> GetAll();
    }
}