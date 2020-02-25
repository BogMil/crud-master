using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCombiners.Implementations
{
    public class Or: IExpressionCombine
    {
        public static string Value => ExpressionCombiner.Or;
        public BinaryExpression Combine(Expression left, Expression right)
        {
            return Expression.Or(left, right);
        }
    }
}
