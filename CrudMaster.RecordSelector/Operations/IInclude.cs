using System.Collections.Generic;
using CrudMaster.RecordSelector.States;

namespace CrudMaster.RecordSelector.Operations
{
    public interface IInclude<TEntity>
    {
        IIncludeState<TEntity> Include(IEnumerable<string> includings);
    }
}