using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ExpressionBuilder.ExpressionCombiners;
using ExpressionBuilder.ExpressionCreators;

namespace ExpressionBuilder
{
    public interface IExpressionBuilder
    {
        Expression Combine(Expression left, string operation, Expression right);
        Expression Create(Expression left, string operation, ConstantExpression right);
        ConstantExpression CreateConstantExpression(string data, Type type);
    }

    public class ExprBuilder : IExpressionBuilder
    {
        public Expression Combine(Expression left, string operation, Expression right)
        {
            var ec = ExpressionCombinerFactory.GetExpressionConnector(operation);
            return ec.Combine(left, right);
        }

        public Expression Create(Expression left, string operation, ConstantExpression right)
        {
            var ec = ExpressionCreatorFactory.Get(operation);
            return ec.Create(left, right);
        }

        public ConstantExpression CreateConstantExpression(string data, Type type)
        {
            dynamic castedData = CastStringDataToType(data, type);
            return Expression.Constant(castedData, type);
        }

        public object CastStringDataToType(string data, Type propertyType)
        {
            var propertyTypeFullName = propertyType.FullName;

            if (propertyTypeFullName == typeof(int).FullName)
            {
                if (int.TryParse(data, out var intData))
                    return intData;
                throw new Exception("Nije moguce izvriti konvertovanje u tip: " + propertyType);
            }

            if (propertyTypeFullName == typeof(string).FullName)
            {
                return data;
            }

            if (propertyTypeFullName == typeof(object).FullName)
            {
                return data as object;
            }


            switch (propertyTypeFullName)
            {
                case "Int32":
                    if (int.TryParse(data, out var intData))
                        return intData;
                    throw new Exception("Nije moguce izvriti konvertovanje u tip: " + propertyType);

                case "Nullable.Int32":
                    if (int.TryParse(data, out var nullableIntData))
                        return nullableIntData;
                    return null;

                case "Int64":
                    if (long.TryParse(data, out var longData))
                        return longData;
                    throw new Exception("Nije moguce izvriti konvertovanje u tip: " + propertyType);

                case "Nullable.Int64":
                    if (long.TryParse(data, out var nullableLongData))
                        return nullableLongData;
                    throw new Exception("Nije moguce izvriti konvertovanje u tip: " + propertyType);

                case "Double":
                    if (double.TryParse(data, out var doubleData))
                        return doubleData;
                    throw new Exception("Nije moguce izvriti konvertovanje u tip: " + propertyType);

                case "Nullable.Double":
                    if (double.TryParse(data, out var nullableDoubleData))
                        return nullableDoubleData;
                    return null;

                case "String":
                    return data;

                case "DateTime":
                    if (DateTime.TryParse(data, out var dateTimeData))
                        return dateTimeData;
                    throw new Exception("Nije moguce izvriti konvertovanje u tip: " + propertyType);

                case "Nullable.DateTime":
                    if (DateTime.TryParse(data, out var nullableDateTimeData))
                        return nullableDateTimeData;
                    return null;


                default:
                    throw new Exception("Nije podrzano kastovanje podatka za pretragu za propertyType : " + propertyType);
            }
        }
    }
}
