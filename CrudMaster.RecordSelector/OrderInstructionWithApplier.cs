using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CrudMaster.RecordSelector;
using CrudMaster.Utils;

namespace CrudMaster.RecordSelector
{
    public class StringOrderByApplier<TEntity> : IOrderApplyer<TEntity>
    {
        public IOrderedQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> queryable,
            OrderInstruction orderInstruction)
            => queryable.OrderBy((Expression<Func<TEntity, string>>)orderInstruction.LambdaExpression);

        public IOrderedQueryable<TEntity> ApplyOrdering(IOrderedQueryable<TEntity> queryable,
            OrderInstruction orderInstruction)
            => queryable.ThenBy((Expression<Func<TEntity, string>>)orderInstruction.LambdaExpression);
    }

    public class Int32OrderByApplier<TEntity> : IOrderApplyer<TEntity>
    {
        public IOrderedQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> queryable,
            OrderInstruction orderInstruction)
            => queryable.OrderBy((Expression<Func<TEntity, int>>)orderInstruction.LambdaExpression);

        public IOrderedQueryable<TEntity> ApplyOrdering(IOrderedQueryable<TEntity> queryable,
            OrderInstruction orderInstruction)
            => queryable.ThenBy((Expression<Func<TEntity, int>>)orderInstruction.LambdaExpression);
    }

    public class Int64OrderByApplier<TEntity> : IOrderApplyer<TEntity>
    {
        public IOrderedQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> queryable,
            OrderInstruction orderInstruction)
            => queryable.OrderBy((Expression<Func<TEntity, long>>)orderInstruction.LambdaExpression);

        public IOrderedQueryable<TEntity> ApplyOrdering(IOrderedQueryable<TEntity> queryable,
            OrderInstruction orderInstruction)
            => queryable.ThenBy((Expression<Func<TEntity, long>>)orderInstruction.LambdaExpression);
    }


    public class OrderingAppliers
    {
        public static Dictionary<Type, IOrderApplyer<TEntity>> Get<TEntity>()
            => new Dictionary<Type, IOrderApplyer<TEntity>>()
            {
                {typeof(string), new StringOrderByApplier<TEntity>()},
                {typeof(int), new Int32OrderByApplier<TEntity>() },
                {typeof(long), new Int64OrderByApplier<TEntity>() }
            };
    }

    public static class OrderInstructionWithAppliersFactory
    {
        public static IEnumerable<OrderInstructionWithAppliers<TEntity>> Create<TEntity>(
            List<OrderInstruction> orderInstructions)
        {
            return orderInstructions.Select(orderInstruction
                => new OrderInstructionWithAppliers<TEntity>(orderInstruction, OrderingAppliers.Get<TEntity>()));
        }
    }

    public interface IOrderApplyer<TEntity>
    {
        IOrderedQueryable<TEntity> ApplyOrdering(IQueryable<TEntity> queryable, OrderInstruction orderInstruction);

        IOrderedQueryable<TEntity> ApplyOrdering(IOrderedQueryable<TEntity> queryable,
            OrderInstruction orderInstruction);

    }

    public class OrderInstructionWithAppliers<TEntity>
    {
        private readonly Dictionary<Type, IOrderApplyer<TEntity>> _apliers;
        private readonly OrderInstruction _orderInstruction;

        public OrderInstructionWithAppliers(OrderInstruction orderInstruction,
            Dictionary<Type, IOrderApplyer<TEntity>> apliers)
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