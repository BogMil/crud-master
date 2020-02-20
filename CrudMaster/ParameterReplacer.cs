using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CrudMaster
{
    public static class ParameterExpressionReplacer
    {
        public static Expression Replace(Expression expression, ParameterExpression parameter)
        {
            return new ReplaceVisitor().Modify(expression, parameter);
        }
    }

    class ReplaceVisitor : ExpressionVisitor
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
