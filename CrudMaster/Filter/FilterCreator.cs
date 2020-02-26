using System;
using System.Collections.Generic;
using System.Linq;
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
            var filterCreator = new FilterCreator<TEntity, TQueryDto>(filters);
            return filterCreator.Create();
        }
    }
    public class FilterCreator<TEntity, TQueryDto> where TEntity : class where TQueryDto : class
    {
        private FilterObject FilterObject { get; set; }
        private readonly ParameterExpression _parameterExpression = Expression.Parameter(typeof(TEntity), "s");
        private readonly IExpressionBuilder _expressionBuilder;

        public FilterCreator(string filters)
        {
            FilterObject = JsonSerializer.Deserialize<FilterObject>(filters);
            _expressionBuilder = new ExprBuilder();
        }

        public Expression<Func<TEntity, bool>> Create()
        {
            return Create(FilterObject);
        }

        public Expression<Func<TEntity, bool>> Create(FilterObject filterObject)
        {
            var expressionsToCombine = new List<Expression>();

            filterObject.Rules?.ForEach(filterRule => expressionsToCombine.Add(CreateExpressionByRule(filterRule)));
            filterObject.Groups?.ForEach(filterGroup => expressionsToCombine.Add(Create(filterGroup).Body));

            var combinedRulesResult = expressionsToCombine.FirstOrDefault();

            for (var i = 1; i < expressionsToCombine.Count; i++)
                combinedRulesResult = _expressionBuilder.Combine(combinedRulesResult, filterObject.GroupOp, expressionsToCombine[i]);

            return Expression.Lambda<Func<TEntity, bool>>(combinedRulesResult, _parameterExpression);
        }

        public Expression CreateExpressionByRule(Rule filterRule)
        {
            var mappingExpression = new MappingExpression<TQueryDto, TEntity>(filterRule.Field);
            mappingExpression.ReplaceParameter(_parameterExpression);

            var value = _expressionBuilder.CreateConstantExpression(filterRule.Data, mappingExpression.ReturnType);

            return _expressionBuilder.Create(mappingExpression.Body, filterRule.Op, value);
        }
    }
}