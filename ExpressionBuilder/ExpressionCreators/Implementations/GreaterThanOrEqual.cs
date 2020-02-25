using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    internal class GreaterThanOrEqual : IExpressionCreate
    {
        public BinaryExpression Create(Expression left, ConstantExpression right)
        {
            return Expression.GreaterThanOrEqual(left, right);
        }
    }
}