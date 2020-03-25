using System.Linq.Expressions;

namespace CrudMaster.Sorter

{
    public interface IOrderByProperties
    {
        string OrderDirection { get; set; }
        string OrderByProperty { get; set; }
        LambdaExpression x { get; set; }
    }
}