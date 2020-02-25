using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    internal class LessThan : IExpressionCreate
    {
        public Expression Create(Expression left, ConstantExpression right)
        {
            return Expression.LessThan(left, right);
        }
    }
}