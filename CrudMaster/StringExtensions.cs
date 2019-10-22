using System;
using Newtonsoft.Json.Linq;

namespace CrudMaster
{
    public static class StringExtensions
    {
        public static JToken TryParseJToken(this string str)
        {
            try
            {
                return JToken.Parse(str);
            }
            catch
            {
                throw new Exception("Can not parse Jtoken from str: "+ str);
            }
        }
    }
}
