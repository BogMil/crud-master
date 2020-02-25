using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCombiners.Implementations
{
    public class Or: IExpressionCombine
    {
        public static string Value => ExpressionCombiner.Or;
        public BinaryExpression Combine(BinaryExpression left, BinaryExpression right)
        {
            return Expression.And(left, right);
        }
    }
}
