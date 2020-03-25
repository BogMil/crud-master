using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CrudMaster.Extensions
{
    public static class LambdaExpressionExtensions
    {
        public static LambdaExpression ReplaceParameter(this LambdaExpression lambdaExpression, ParameterExpression parameterExpression)
            => ParameterExpressionReplacer.Replace(lambdaExpression, parameterExpression) as LambdaExpression;
    }
}
