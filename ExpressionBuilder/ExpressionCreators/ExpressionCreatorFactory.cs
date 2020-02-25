using System;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.ExpressionCreators.Implementations;

namespace ExpressionBuilder.ExpressionCreators
{
    internal static class ExpressionCreatorFactory
    {

        public static IExpressionCreate Get(string operation)
        {
            operation = operation.ToUpper();

            return operation switch
            {
                Operation.Equal => new Equal(),
                Operation.NotEqual => new NotEqual(),
                Operation.LessThan => new LessThan(),
                Operation.LessThanOrEqual => new LessThanOrEqual(),
                Operation.GreaterThan => new GreaterThan(),
                Operation.GreaterThanOrEqual => new GreaterThanOrEqual(),
                Operation.StartsWith => throw new NotImplementedException(),
                Operation.EndsWith => throw new NotImplementedException(),
                Operation.Contains=> throw new NotImplementedException(),

                _ => throw new Exception()
            };
        }
    }

    //internal class Contains : IExpressionCreate
    //{
    //    public BinaryExpression Create(BinaryExpression left, ConstantExpression right)
    //    {
    //        Expression.Cont
    //    }
    //}

    internal class StartsWith : IExpressionCreate
    {
        private readonly MethodInfo _startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        public BinaryExpression Create(Expression left, ConstantExpression right)
        {
            var s = "s";
            var value = right.Value as string;
            return Expression.Call(left, _startsWithMethod, right);
        }
    }
}
