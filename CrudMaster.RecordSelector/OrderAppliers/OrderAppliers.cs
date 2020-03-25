using System;
using System.Collections.Generic;

namespace CrudMaster.RecordSelector.OrderAppliers
{
    public class OrderAppliers
    {
        public static Dictionary<Type, IOrderApplier<TEntity>> Get<TEntity>()
            => new Dictionary<Type, IOrderApplier<TEntity>>()
            {
                {typeof(string), new StringOrderApplier<TEntity>()},
                {typeof(int), new Int32OrderApplier<TEntity>() },
                {typeof(long), new Int64OrderApplier<TEntity>() }
            };
    }
}