using System;
using System.Collections.Generic;
using System.Linq;
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

        public TemplateWithColumnNames( string template, string columnTemplateRegexPattern = @"{(.*?)}")
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

        public string[] GetColumnNames()
        {
            return Matches.Select(x => x.Groups[1].Value).Distinct().ToArray();
        }

        public string Replace(Dictionary<string,string> pairs)
        {
            var template = Template;
            foreach (var pair in pairs)
            {
                template = template.Replace(pair.Key, pair.Value);
            }

            return template;
        }

        public string Replace(object entity, Dictionary<string,string> templateToColNamesDictionary)
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
    }
}
