using System;
using System.Reflection;

namespace CrudMaster.Sorter

{
    public interface IOrderByProperties
    {
        string OrderDirection { get; set; }
        string OrderByProperty { get; set; }
    }
}