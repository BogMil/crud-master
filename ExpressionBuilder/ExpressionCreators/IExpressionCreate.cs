using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionBuilder.ExpressionCreators
{
    interface IExpressionCreate
    {
        BinaryExpression Create(Expression left, ConstantExpression right);
    }
}
