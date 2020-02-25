using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    class Equal:IExpressionCreate
    {
        public BinaryExpression Create(Expression left, ConstantExpression right)
        {
            return Expression.Equal(left, right);
        }
    }
}
