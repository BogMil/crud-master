using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CrudMaster
{
    public class ExpressionString
    {
        public string Value { get; }
        public string ParameterName { get; private set; }

        public List<string> TableNamesToInclude = new List<string>();

        private Regex _navigationWithFunctionCallRegex = new Regex(@"(\w+(\.\w+)+)(?=[(])");
        private Regex _navigationWithoutFunctionCallRegex = new Regex(@"(\w+(\.\w+)+)(?=\s+)");
        private Regex _regex = new Regex(@"(\w+(\.\w+)+)[(]*");

        public ExpressionString(string expStr)
        {
            Value = expStr;
            SetParameterName();

            var navigations = _regex.Matches(Value).Select(s => s.Value).ToList();

            var list = new List<string>();

            foreach (var str in navigations)
            {
                var navigation = str;
                navigation = RemoveParameterName(navigation);

                if (!navigation.Contains("."))
                    continue;

                if (navigation.Contains("("))
                {
                    navigation = RemoveFromLastDotToEnd(navigation);
                }

                if (!navigation.Contains("."))
                    continue;

                navigation = RemoveFromLastDotToEnd(navigation);
                if (!TableNamesToInclude.Contains(navigation))
                    TableNamesToInclude.Add(navigation);


            }
        }

        private void TablesToIncludeFromCallExp()
        {
            var navigationWithFunctionCallRegex = _navigationWithFunctionCallRegex.Matches(Value).Select(s=>RemoveFromLastDotToEnd(s.Value)).ToList();
            
            var list = new List<string>();

            foreach (var str in navigationWithFunctionCallRegex)
            {
                var s = RemoveFromLastDotToEnd(str);
                s = RemoveParameterName(s);
                if (s=="")
                    continue; 
                if(!list.Contains(s))
                    list.Add(s);
            }
            PopulateTableNamesToInclude(list);
        }

        private void TablesToIncludeFromNonCallExp()
        {
            var navigationWithoutFunctionCallRegex = _navigationWithoutFunctionCallRegex.Matches(Value).Select(x => x.Value).ToList(); ;

            var list = new List<string>();

            foreach (var str in navigationWithoutFunctionCallRegex)
            {
                var s = RemoveFromLastDotToEnd(str);
                s = RemoveParameterName(s);
                if (s == "")
                    continue;
                if (!list.Contains(s))
                    list.Add(s);
            }
            PopulateTableNamesToInclude(list);
        }

        private void PopulateTableNamesToInclude(List<string> listOfNavigations)
        {
            foreach (var navigation in listOfNavigations)
            {
                if (!TableNamesToInclude.Contains(navigation))
                    TableNamesToInclude.Add(navigation);
            }
        }

        
        private string RemoveFromLastDotToEnd(string str)
        {
            if (!str.Contains("."))
                return "";
            var lastPosOfDot = str.LastIndexOf(".");
            var x= str.Substring(0, lastPosOfDot);
            return x;
        }

        private string RemoveParameterName(string str)
        {
            if (!str.Contains("."))
                return "";
            var x =str.Substring(str.IndexOf(".")+1);
            return x;
        }

        private void SetParameterName()
        {
            var indexOfparameterName = Value.IndexOf("=>", StringComparison.Ordinal);
            ParameterName = Value.Substring(0, indexOfparameterName);
        }
    }
}
