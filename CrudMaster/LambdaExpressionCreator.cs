using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CrudMaster
{
    public class LambdaExpressionCreator<TEntity>
    {
        public readonly Type EntityType = typeof(TEntity);
        public readonly Type ExpressionsFuncReturnType;
        //do not change name
        public readonly LambdaExpression LambdaExpression;
        public readonly ParameterExpression ExpressionInputParameter;
        public readonly Expression Expression;
        public readonly Type DelegateType;
        public readonly string FullPropertyPath;
        /// <summary>
        /// Without parameter
        /// </summary>
        /// <param name="fullPropertyPath"></param>

        public LambdaExpressionCreator(string fullPropertyPath)
        {
            FullPropertyPath = fullPropertyPath;
            ExpressionInputParameter = Expression.Parameter(EntityType, "x");

            var expressionBuiltFromString = CreateExpressionBuiltFromString();
            ExpressionsFuncReturnType = expressionBuiltFromString.ExpressionsFuncReturnType;
            Expression = expressionBuiltFromString.Expression;
            DelegateType = typeof(Func<,>).MakeGenericType(EntityType, ExpressionsFuncReturnType);
            LambdaExpression = CreateLambdaExpression();
        }

        private ExpressionBuiltFromString CreateExpressionBuiltFromString()
        {
            string[] splitProperyPath = FullPropertyPath.Split('.');
            var expressionsFuncReturnType = EntityType;
            Expression expr = ExpressionInputParameter;

            foreach (string prop in splitProperyPath)
            {
                PropertyInfo pi = expressionsFuncReturnType.GetProperty(prop);
                if (pi == null)
                    throw new MissingFieldException($"Type '{EntityType}' does not have property '{prop}'");
                expr = Expression.Property(expr, pi);
                expressionsFuncReturnType = pi.PropertyType;
            }

            return new ExpressionBuiltFromString()
            {
                Expression = expr,
                ExpressionsFuncReturnType = expressionsFuncReturnType
            };
        }

        private LambdaExpression CreateLambdaExpression()
        {
            return Expression.Lambda(DelegateType, Expression, ExpressionInputParameter);
        }

        private class ExpressionBuiltFromString
        {
            public Expression Expression { get; set; }
            public Type ExpressionsFuncReturnType { get; set; }
        }
    }
}
