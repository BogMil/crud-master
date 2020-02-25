using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    internal class GreaterThan : IExpressionCreate
    {
        public BinaryExpression Create(Expression left, ConstantExpression right)
        {
            return Expression.GreaterThan(left, right);
        }
    }
}