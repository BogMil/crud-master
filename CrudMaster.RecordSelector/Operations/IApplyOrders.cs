using System.Collections.Generic;
using CrudMaster.RecordSelector.States;
using CrudMaster.Utils;

namespace CrudMaster.RecordSelector.Operations
{
    public interface IApplyOrders<TEntity>
    {
        IApplyOrdersState<TEntity> ApplyOrders(List<OrderInstruction> orderByInstructions);
    }
}