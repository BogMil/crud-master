using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionBuilder.ExpressionCombiners
{
    public interface IExpressionCombine
    {
        BinaryExpression Combine(Expression left, Expression right);
    }
}
