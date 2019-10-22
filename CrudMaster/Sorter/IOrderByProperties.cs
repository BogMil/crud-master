using System;
using System.Reflection;

namespace CrudMaster.Sorter

{
    public interface IOrderByProperties<TEntity>
    {
        Func<TEntity,dynamic> OrderByColumnFunc { get; set; }
        string OrderDirection { get; set; }
        PropertyInfo TestPropertyInfo { get; set; }
        string OrderByProperty { get; set; }
    }
}