using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

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
        public List<string> PathsForRelatedTableToinclude= new List<string>();

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
