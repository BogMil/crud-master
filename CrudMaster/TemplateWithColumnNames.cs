﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Internal;

namespace CrudMaster
{
    public class TemplateWithColumnNames
    {
        public string ColumnTemplateRegexPattern;
        public string Template;
        public Regex Regex;
        public string[] ColumnNames { get; set; }
        public MatchCollection Matches;
        public Dictionary<string, LambdaExpression> ExpressionsOfDtoToEntityColNames { get; set; } = new Dictionary<string, LambdaExpression>();

        public TemplateWithColumnNames(string template, string columnTemplateRegexPattern = @"{(.*?)}")
        {
            Template = template;
            var regex = new Regex(columnTemplateRegexPattern);
            Matches = regex.Matches(template);
        }

        public string GetWithLowerizedColumnNames()
        {
            var template = Template;
            foreach (Match match in Matches)
            {
                template = template.Replace(match.Value, match.Value.ToLower());
            }

            return template;
        }

        public string[] GetDtoColumnNames()
        {
            return Matches.Select(x => x.Groups[1].Value).Distinct().ToArray();
        }

        public string Replace(Dictionary<string, string> pairs)
        {
            var template = Template;
            foreach (var pair in pairs)
            {
                template = template.Replace(pair.Key, pair.Value);
            }

            return template;
        }

        public string Replace(object entity)
        {
            var template = GetWithLowerizedColumnNames();
            foreach (var pair in ExpressionsOfDtoToEntityColNames)
            {
                var dtoColNameTemplate = $"{{{pair.Key.ToLower()}}}";
                template = template.Replace(dtoColNameTemplate, ExecuteExpressionOnObject(entity,pair.Value));
            }

            return template;
        }

        public string Replace(object entity, Dictionary<string, string> templateToColNamesDictionary)
        {
            var template = GetWithLowerizedColumnNames();
            foreach (var (dtoColName, entityColName) in templateToColNamesDictionary)
            {
                var dtoColNameTemplate = $"{{{dtoColName.ToLower()}}}";
                var colValue = entity.GetType().GetProperty(entityColName)?.GetValue(entity).ToString();
                template = template.Replace(dtoColNameTemplate, colValue);
            }

            return template;
        }

        public dynamic Test(dynamic entity, List<LambdaExpression> exps)
        {
            //return entity.Region.Name.ToString();
            string s = "";
            foreach (var exp in exps)
            {
                var compiledLambda = exp.Compile();
                var result = compiledLambda.DynamicInvoke(entity);
                s += result.ToString() + " ";
            }

            return s;
            //return entity;
        }
        public dynamic ExecuteExpressionOnObject(object entity, LambdaExpression exp)
        {
            //return entity.Region.Name.ToString();
            var compiledLambda = exp.Compile();
            var result = compiledLambda.DynamicInvoke(entity);
            var s = result.ToString();
            return s;
        }
    }
}
