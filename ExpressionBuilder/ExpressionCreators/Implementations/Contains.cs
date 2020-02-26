using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    internal class Contains : IExpressionCreate
    {
        private readonly MethodInfo _startsWithMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        public Expression Create(Expression left, ConstantExpression right)
        {
            return Expression.Call(left, _startsWithMethod, right);
        }
    }
}