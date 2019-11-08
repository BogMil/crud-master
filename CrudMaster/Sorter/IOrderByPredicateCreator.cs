namespace CrudMaster.Sorter

{
    public interface IOrderByPredicateCreator
    {
        IOrderByProperties GetPropertyObject(OrderByProperties orderByProperties);
    }
}