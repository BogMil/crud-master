using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrudMaster.RecordSelector
{
    public static class EnumerableExtensions
    {
        public static bool IsFirst<T>(this IEnumerable<T> items, T item)
        {
            var first = items.FirstOrDefault();
            return first != null && item.Equals(first);
        }
    }
}
