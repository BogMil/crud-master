using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace CrudMaster
{
    public class StringifiedExpression
    {
        private readonly string _stringifiedExpression;
        private readonly string _expressionParameter;
        private readonly Regex _parameterExpresionUsagesRegex;

        public StringifiedExpression(LambdaExpression lambdaExpression)
        {
            var strExp = lambdaExpression.ToString();
            _stringifiedExpression = Regex.Replace(strExp, @"\s+", "");
            _expressionParameter = GetExpressionParameterAsString();
            _parameterExpresionUsagesRegex
                = new Regex($@"{_expressionParameter}\.(\w*(\.\w*)*)([^\s\+\-\*\/\)}}{{])");
        }

        public IEnumerable<string> GetIncludings()
        {
            return GetPosibleSourcesOfIncludings()
                .Select(x => x.Replace($"{_expressionParameter}.", ""))
                .Where(x => x.Contains('.'))
                .Select(x => x.Remove(x.LastIndexOf('.')))
                .Distinct()
                .ToHashSet();
        }

        private List<string> GetPosibleSourcesOfIncludings() =>
            _parameterExpresionUsagesRegex
                .Matches(_stringifiedExpression)
                .Select(s => s.Value[^1] == '(' ? s.Value.Remove(s.Value.LastIndexOf('.')) : s.Value)
                .ToList();

        private string GetExpressionParameterAsString()
        {
            var indexOfparameterName = _stringifiedExpression.IndexOf("=>", StringComparison.Ordinal);
            return _stringifiedExpression.Substring(0, indexOfparameterName).Trim();
        }
    }
}