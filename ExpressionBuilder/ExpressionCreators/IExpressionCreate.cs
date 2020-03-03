using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators
{
    interface IExpressionCreate
    {
        Expression Create(Expression left, ConstantExpression right);
    }
}
