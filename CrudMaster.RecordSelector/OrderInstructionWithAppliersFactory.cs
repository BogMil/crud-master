using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrudMaster.RecordSelector;
using CrudMaster.Utils;

namespace CrudMaster.RecordSelector
{
    public static class OrderInstructionWithAppliersFactory
    {
        public static IEnumerable<OrderInstructionWithAppliers<TEntity>> Create<TEntity>(
            List<OrderInstruction> orderInstructions)
        {
            return orderInstructions.Select(orderInstruction
                => new OrderInstructionWithAppliers<TEntity>(orderInstruction, OrderAppliers.OrderAppliers.Get<TEntity>()));
        }
    }
}