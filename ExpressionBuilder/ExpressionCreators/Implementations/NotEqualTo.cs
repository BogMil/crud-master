using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    class NotEqual : IExpressionCreate
    {
        public Expression Create(Expression left, ConstantExpression right)
        {
            return Expression.NotEqual(left, right);
        }
    }
}
