using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCombiners.Implementations
{
    public class And: IExpressionCombine
    {
        public static string Value => ExpressionCombiner.And;
        public BinaryExpression Combine(Expression left, Expression right)
        {
            return Expression.And(left, right);
        }
    }
}
