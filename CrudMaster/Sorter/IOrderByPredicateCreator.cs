namespace CrudMaster.Sorter

{
    public interface IOrderByPredicateCreator<TEntity>
    {
        IOrderByProperties<TEntity> GetPropertyObject(OrderByProperties orderByProperties);
    }
}