using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    internal class GreaterThanOrEqual : IExpressionCreate
    {
        public Expression Create(Expression left, ConstantExpression right)
        {
            return Expression.GreaterThanOrEqual(left, right);
        }
    }
}