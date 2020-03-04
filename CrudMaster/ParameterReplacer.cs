using System.Linq.Expressions;

namespace CrudMaster
{
    public static class ParameterExpressionReplacer
    {
        public static Expression Replace(Expression expression, ParameterExpression parameter)
        {
            return new ReplaceVisitor().Modify(expression, parameter);
        }
    }

    public class ReplaceVisitor : ExpressionVisitor
    {
        private ParameterExpression _parameter;

        public Expression Modify(Expression expression, ParameterExpression parameter)
        {
            _parameter = parameter;
            return Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameter;
        }
    }
}
