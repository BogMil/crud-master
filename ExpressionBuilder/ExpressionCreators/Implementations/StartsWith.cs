using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    internal class StartsWith : IExpressionCreate
    {
        private readonly MethodInfo _startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        public Expression Create(Expression left, ConstantExpression right)
        {
            return Expression.Call(left, _startsWithMethod, right);
        }
    }
}