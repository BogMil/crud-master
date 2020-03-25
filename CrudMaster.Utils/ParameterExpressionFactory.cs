using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CrudMaster.Utils
{
    public static class ParameterExpressionFactory
    {
        /// <summary>
        /// Creates ParameterExpression of provided type
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static ParameterExpression Create<TEntity>()
            => Expression.Parameter(typeof(TEntity), Constants.PARAMETER_EXPRESSION_NAME);
    }
}
