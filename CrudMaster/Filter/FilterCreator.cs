using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using ExpressionBuilder;

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
        private readonly IMappingService _mappingService;
        private readonly ParameterExpression _parameterExpression= Expression.Parameter(typeof(TEntity), "s");
        private readonly IExpressionBuilder _expressionBuilder;

        public FilterCreator(string filters )
        {
            FilterObject = JsonSerializer.Deserialize<FilterObject>(filters);
            _mappingService = new MappingService();
            _expressionBuilder= new ExprBuilder();
        }

        public Expression<Func<TEntity, bool>> Create()
        {
            return Create(FilterObject);
        }

        public Expression<Func<TEntity, bool>> Create(FilterObject filterObject)
        {
            List<BinaryExpression> expressionsToCombine= new List<BinaryExpression>();

            foreach (var filterRule in filterObject.Rules)
            {
                var mappingExpression = new MappingExpression<TQueryDto, TEntity>(filterRule.Field);
                mappingExpression.ReplaceParameter(_parameterExpression);

                ConstantExpression value = CreateConstantExpression(filterRule.Data, mappingExpression.ReturnType);

                //BinaryExpression binaryExpression = Expression.Equal(mappingExpression.Body, value);
                BinaryExpression binaryExpression = _expressionBuilder.Create(mappingExpression.Body,filterRule.Op ,value);
                expressionsToCombine.Add(binaryExpression);
            }

            var res = _expressionBuilder.Combine(expressionsToCombine[0], filterObject.GroupOp, expressionsToCombine[1]);
            var res2 = _expressionBuilder.Combine(res, filterObject.GroupOp,expressionsToCombine[2]);

            return Expression.Lambda<Func<TEntity, bool>>(res2, _parameterExpression);
        }

        public ConstantExpression CreateConstantExpression(string data,Type type)
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
    }
}