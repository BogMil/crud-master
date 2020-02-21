using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CrudMaster
{
    public static class ExpressionConnectors
    {
        public static IExpressionConnector GetExpressionConnector(string v)
        {
            if(AndExpressionConnector.Value==v)
                return new AndExpressionConnector();

            if (OrExpressionConnector.Value == v)
                return new OrExpressionConnector();

            throw new Exception("Nepostojeca operacija");
        }
    }

    public interface IExpressionConnector
    {
        BinaryExpression ConnectExpressions(BinaryExpression left, BinaryExpression right);
    }
    public class AndExpressionConnector : IExpressionConnector
    {
        public static string Value => "AND";
        public BinaryExpression ConnectExpressions(BinaryExpression left, BinaryExpression right)
        {
            return Expression.And(left, right);
        }
    }

    public class OrExpressionConnector : IExpressionConnector
    {
        public static string Value => "OR";
        public BinaryExpression ConnectExpressions(BinaryExpression left, BinaryExpression right)
        {
            return Expression.And(left, right);
        }
    }
}
