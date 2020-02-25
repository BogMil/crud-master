using System;
using ExpressionBuilder.ExpressionCombiners.Implementations;

namespace ExpressionBuilder.ExpressionCombiners
{
    internal static class ExpressionCombinerFactory
    {
        public static IExpressionCombine GetExpressionConnector(string operation)
        {
            operation = operation.ToUpper();
            if (And.Value == operation)
                return new And();

            if (Or.Value == operation)
                return new Or();

            throw new Exception("Nepostojeca operacija");
        }
    }
}
