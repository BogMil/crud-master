using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using CrudMaster.Extensions;
using CrudMaster.Utils;
using ExpressionBuilder;
using static System.String;

namespace CrudMaster.Filter
{


    public static class WherePredicateFactory
    {
        public static Expression<Func<TEntity, bool>> Create<TEntity, TQueryDto>(string filters)
            where TEntity : class
            where TQueryDto : class
                => new WherePredicateCreator<TEntity, TQueryDto>(filters)
                    .Create();
    }
    public class WherePredicateCreator<TEntity, TQueryDto> 
        where TEntity : class 
        where TQueryDto : class
    {
        private FilterObject FilterObject { get; }
        private readonly ParameterExpression _parameterExpression;
        private readonly IExpressionBuilder _expressionBuilder;

        public WherePredicateCreator(string filters)
        {
            FilterObject = new FilterObject();
            _expressionBuilder = new ExprBuilder();
            _parameterExpression = ParameterExpressionFactory.Create<TEntity>();

            if (!IsNullOrEmpty(filters))
                FilterObject = JsonSerializer.Deserialize<FilterObject>(filters);
        }

        public Expression<Func<TEntity, bool>> Create() => Create(FilterObject);
        public Expression<Func<TEntity, bool>> Create(FilterObject filterObject)
        {
            var expressionsToCombine = new List<Expression>();

            filterObject.Rules
                ?.ForEach(
                    filterRule => expressionsToCombine.Add(CreateExpressionByRule(filterRule)));

            filterObject.Groups?.ForEach(filterGroup => expressionsToCombine.Add(Create(filterGroup).Body));

            var combinedRulesResult = expressionsToCombine.FirstOrDefault();

            for (var i = 1; i < expressionsToCombine.Count; i++)
                combinedRulesResult = _expressionBuilder.Combine(combinedRulesResult, filterObject.GroupOp, expressionsToCombine[i]);

            return combinedRulesResult != null ? Expression.Lambda<Func<TEntity, bool>>(combinedRulesResult, _parameterExpression) : null;
        }

        public Expression CreateExpressionByRule(Rule filterRule)
        {
            var ms = new MappingService();
            var lambdaExpression = ms.GetPropertyMapExpression(filterRule.Field, typeof(TQueryDto), typeof(TEntity))
                .ReplaceParameter(_parameterExpression);

            var value = _expressionBuilder.CreateConstantExpression(filterRule.Data, lambdaExpression.ReturnType);
            return _expressionBuilder.Create(lambdaExpression.Body, filterRule.Op, value);
        }
    }
}