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
        BinaryExpression Combine(BinaryExpression left, string operation, BinaryExpression right);
        BinaryExpression Create(Expression left, string operation, ConstantExpression right);
    }

    public class ExprBuilder : IExpressionBuilder
    {
        public BinaryExpression Combine(BinaryExpression left, string operation, BinaryExpression right)
        {
            var ec = ExpressionCombinerFactory.GetExpressionConnector(operation);
            return ec.Combine(left, right);
        }

        public BinaryExpression Create(Expression left, string operation, ConstantExpression right)
        {
            var ec = ExpressionCreatorFactory.Get(operation);
            return ec.Create(left, right);
        }
    }
}
