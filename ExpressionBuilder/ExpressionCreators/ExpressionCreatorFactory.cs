using System;
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
                Operation.StartsWith => new StartsWith(),
                Operation.EndsWith => new EndsWidth(),
                Operation.Contains=> new Contains(),

                _ => throw new Exception()
            };
        }
    }
}
