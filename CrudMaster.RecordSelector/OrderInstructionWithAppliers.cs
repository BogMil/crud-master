using System;
using System.Collections.Generic;
using System.Linq;
using CrudMaster.RecordSelector.OrderAppliers;
using CrudMaster.Utils;

namespace CrudMaster.RecordSelector
{
    public class OrderInstructionWithAppliers<TEntity>
    {
        private readonly Dictionary<Type, IOrderApplier<TEntity>> _apliers;
        private readonly OrderInstruction _orderInstruction;

        public OrderInstructionWithAppliers(OrderInstruction orderInstruction,
            Dictionary<Type, IOrderApplier<TEntity>> apliers)
        {
            _orderInstruction = orderInstruction;
            _apliers = apliers;
        }

        public IOrderedQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> queryable)
            => _apliers[_orderInstruction.LambdaExpression.ReturnType].ApplyOrdering(queryable, _orderInstruction);

        public IOrderedQueryable<TEntity> ApplyOrdering(IOrderedQueryable<TEntity> queryable)
            => _apliers[_orderInstruction.LambdaExpression.ReturnType].ApplyOrdering(queryable, _orderInstruction);
    }
}