﻿using System.Linq.Expressions;

namespace ExpressionBuilder.ExpressionCreators.Implementations
{
    internal class LessThanOrEqual : IExpressionCreate
    {
        public Expression Create(Expression left, ConstantExpression right)
        {
            return Expression.LessThanOrEqual(left, right);
        }
    }
}