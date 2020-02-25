using System;
using ExpressionBuilder.ExpressionCombiners.Implementations;

namespace ExpressionBuilder.ExpressionCombiners
{
    internal static class ExpressionCombinerFactory
    {
        public static IExpressionCombine GetExpressionConnector(string v)
        {
            v = v.ToUpper();
            if (And.Value == v)
                return new And();

            if (Or.Value == v)
                return new Or();

            throw new Exception("Nepostojeca operacija");
        }
    }
}
