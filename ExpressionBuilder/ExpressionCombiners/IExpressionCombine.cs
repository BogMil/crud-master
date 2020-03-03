using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCombiners
{
    public interface IExpressionCombine
    {
        BinaryExpression Combine(Expression left, Expression right);
    }
}
