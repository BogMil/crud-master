using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CrudMaster.Extensions
{
    public static class StringExtensions
    {
        public static string ToUpperFirsLetter(this string str)
        {
            if (str == null)
                return null;
            if (str.Length == 1)
                return str.ToUpper();

            return str.First().ToString().ToUpper() + str.Substring(1);
        }
        public static JToken TryParseJToken(this string str)
        {
            try
            {
                return JToken.Parse(str);
            }
            catch
            {
                throw new Exception("Can not parse Jtoken from str: " + str);
            }
        }
    }
}
