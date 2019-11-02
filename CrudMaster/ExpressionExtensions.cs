using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CrudMaster
{
    public static class ExpressionExtensions
    {
        //Expression body of x.y.z. is y.z - Everything that comes after first dot
        public static string GetExpressionBodyAsString<T, TProperty>(this Expression<Func<T, TProperty>> exp)
        {
            var expressionBody = exp.Body;
            var expressionBodyProperties = GetStackOfExpressionBodyProperties(expressionBody);
            var expressionBodyAsString = string.Join(".", expressionBodyProperties.ToArray());
            return expressionBodyAsString;
        }

        public static string NonExtenionGetExpressionBodyAsString<T, TProperty>(Expression<Func<T, TProperty>> exp)
        {
            var expressionBody = exp.Body;
            var expressionBodyProperties = GetStackOfExpressionBodyProperties(expressionBody);
            var expressionBodyAsString = string.Join(".", expressionBodyProperties.ToArray());
            return expressionBodyAsString;
        }

        private static Stack<string> GetStackOfExpressionBodyProperties(Expression expression)
        {
            var propertyNames = new Stack<string>();

            while (expression.HasMemberExpression())
            {
                //propertyNames.Push(((MemberExpression)expression).Member.Name);
                //expression = ((MemberExpression)expression).Expression;

                if (expression is MemberExpression)
                {
                    propertyNames.Push(((MemberExpression)expression).Member.Name);
                    expression = ((MemberExpression)expression).Expression;

                }
                else
                {
                    var op = ((UnaryExpression)expression).Operand;
                    propertyNames.Push(((MemberExpression)op).Member.Name);
                    expression = ((MemberExpression)op).Expression;

                }
            }

            return propertyNames;
        }

        private static bool HasMemberExpression(this Expression exp)
        {
            if (exp == null)
                return false;

            if (exp as MemberExpression != null)
                return true;

            if (IsConversion(exp) && exp is UnaryExpression)
            {
                exp = (exp as UnaryExpression).Operand as MemberExpression;
                if (exp != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsConversion(Expression exp)
        {
            if (exp.NodeType == ExpressionType.Call)
                throw new Exception("Not used case");
            return (
                exp.NodeType == ExpressionType.Convert ||
                exp.NodeType == ExpressionType.ConvertChecked
            );
        }
    }
}