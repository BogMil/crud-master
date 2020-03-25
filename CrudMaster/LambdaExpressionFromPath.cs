using System;
using System.Linq.Expressions;
using CrudMaster.Utils;

namespace CrudMaster
{
    public static class LambdaExpressionFromPathFactory
    {
        public static dynamic CreateInRuntime(Type type, string path)
        {
            var concreteTypeLambdaExpessionFromPath = typeof(LambdaExpressionFromPath<>).MakeGenericType(type);
            return Activator.CreateInstance(concreteTypeLambdaExpessionFromPath, path);
        }
    }
    public class LambdaExpressionFromPath<TSource>
    {
        public readonly Type ExpressionFuncReturnType;
        public readonly LambdaExpression LambdaExpression;
        public readonly ParameterExpression ParameterExpression;
        public readonly Expression Expression;
        public readonly string FullPropertyPath;
        /// <summary>
        /// Without parameter
        /// </summary>
        /// <param name="fullPropertyPath"></param>

        public LambdaExpressionFromPath(string fullPropertyPath)
        {
            FullPropertyPath = fullPropertyPath;
            ParameterExpression = ParameterExpressionFactory.Create<TSource>();
            var expressionBuiltFromString = CreateExpressionBuiltFromString();
            ExpressionFuncReturnType = expressionBuiltFromString.ExpressionsFuncReturnType;
            Expression = expressionBuiltFromString.Expression;
            LambdaExpression = CreateLambdaExpression();
        }

        private ExpressionBuiltFromString CreateExpressionBuiltFromString()
        {
            var splitProperyPath = FullPropertyPath.Split('.');
            var expressionFuncReturnType = typeof(TSource);
            Expression expr = ParameterExpression;

            foreach (var prop in splitProperyPath)
            {
                var property = expressionFuncReturnType.GetProperty(prop);
                if (property == null)
                    throw new MissingFieldException($"'{typeof(TSource)}' does not have property '{prop}'. Check mappings!");
                expr = Expression.Property(expr, property);
                expressionFuncReturnType = property.PropertyType;
            }

            return new ExpressionBuiltFromString()
            {
                Expression = expr,
                ExpressionsFuncReturnType = expressionFuncReturnType
            };
        }

        private LambdaExpression CreateLambdaExpression()
        {
            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TSource), ExpressionFuncReturnType);
            return Expression.Lambda(delegateType, Expression, ParameterExpression);
        }

        private class ExpressionBuiltFromString
        {
            public Expression Expression { get; set; }
            public Type ExpressionsFuncReturnType { get; set; }
        }
    }
}
