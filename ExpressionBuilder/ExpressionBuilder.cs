using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ExpressionBuilder.ExpressionCombiners;
using ExpressionBuilder.ExpressionCreators;

namespace ExpressionBuilder
{
    public interface IExpressionBuilder
    {
        Expression Combine(Expression left, string operation, Expression right);
        Expression Create(Expression left, string operation, ConstantExpression right);
    }

    public class ExprBuilder : IExpressionBuilder
    {
        public Expression Combine(Expression left, string operation, Expression right)
        {
            var ec = ExpressionCombinerFactory.GetExpressionConnector(operation);
            return ec.Combine(left, right);
        }

        public Expression Create(Expression left, string operation, ConstantExpression right)
        {
            var ec = ExpressionCreatorFactory.Get(operation);
            return ec.Create(left, right);
        }
    }
}
