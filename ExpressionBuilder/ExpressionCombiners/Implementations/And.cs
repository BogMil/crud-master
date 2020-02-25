using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCombiners.Implementations
{
    public class And: IExpressionCombine
    {
        public static string Value => ExpressionCombiner.And;
        public BinaryExpression Combine(BinaryExpression left, BinaryExpression right)
        {
            return Expression.And(left, right);
        }
    }
}
