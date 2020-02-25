using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    internal class GreaterThan : IExpressionCreate
    {
        public Expression Create(Expression left, ConstantExpression right)
        {
            return Expression.GreaterThan(left, right);
        }
    }
}