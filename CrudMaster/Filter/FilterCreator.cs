using System;
using System.Collections.Generic;
using System.Reflection;
using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Operations;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Text.Json;

namespace CrudMaster.Filter
{
    

    public static class FilterFactory
    {
        public static Expression<Func<TEntity, bool>> Create<TEntity, TQueryDto>(string filters) where TEntity : class where TQueryDto : class
        {
            var filterCreator = new FilterCreator<TEntity,TQueryDto>(filters);
            return filterCreator.Create();
        }
    }
    public class FilterCreator<TEntity,TQueryDto> where TEntity : class where TQueryDto: class
    {
        private FilterObject FilterObject { get; set; }
        //public JToken JsonFilters{ get; set; }
        private readonly IMappingService _mappingService;
        public FilterCreator(string filters )
        {
            FilterObject = JsonSerializer.Deserialize<FilterObject>(filters);
            _mappingService = new MappingService();
        }
        public Expression<Func<TEntity, bool>> Create()
        {
            ParameterExpression parameterForAllExpressions = Expression.Parameter(typeof(TEntity), "s");
            List<BinaryExpression> expressionsToAnd = new List<BinaryExpression>();
            foreach (var filterRule in FilterObject.Rules)
            {
                var mappingExpression = new MappingExpression<TQueryDto, TEntity>(filterRule.Field);
                mappingExpression.ReplaceParameter(parameterForAllExpressions);

                ConstantExpression value =
                    CreateConstantExpressionFromStringData(filterRule.Data, mappingExpression.ReturnType);

                BinaryExpression binaryExpression = Expression.Equal(mappingExpression.Body, value);
                expressionsToAnd.Add(binaryExpression);
            }

            var expressionConnector = ExpressionConnectors.GetExpressionConnector(FilterObject.GroupOp);
            var res = expressionConnector.Connect(expressionsToAnd[0], expressionsToAnd[1]);
            var res2 = expressionConnector.Connect(res, expressionsToAnd[2]);

            return Expression.Lambda<Func<TEntity, bool>>(res2, parameterForAllExpressions);
        }

        public ConstantExpression CreateConstantExpressionFromStringData(string data,Type type)
        {
            dynamic castedData= GetValidDataByOperationType(data, type);
            return Expression.Constant(castedData, type);
        }

        public object GetValidDataByOperationType(string data, Type propertyType)
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


        public IOperation GetOperationByString(string operation)
        {
            try
            {
                switch (operation)
                {
                    case "eq":
                        return Operation.EqualTo;
                    case "ne":
                        return Operation.NotEqualTo;
                    case "lt":
                        return Operation.LessThan;
                    case "le":
                        return Operation.LessThanOrEqualTo;
                    case "gt":
                        return Operation.GreaterThan;
                    case "ge":
                        return Operation.GreaterThanOrEqualTo;
                    case "bw":
                        return Operation.StartsWith;
                    case "ew":
                        return Operation.EndsWith;
                    case "cn":
                        return Operation.Contains;
                    case "nc":
                        return Operation.DoesNotContain;
                    default:
                        throw new Exception("Nepostojeca operacija " + operation + ". Dozvoljene operacije su:eq,ne,lt,lt,gt,ge,bw,ew,cn,nc.");
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                throw e;
            }

        }
    }
}