using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CrudMaster.Extensions;

namespace CrudMaster
{
    public class IncludingsExtractor
    {
        public IEnumerable<string> Extract(TypeMap typeMap)
        {
            var res = new List<string>();

            typeMap
                .GetCustomMapExpressions()
                .ForEach(s => res.AddRange(new StringifiedExpression(s).GetIncludings()));
            return res.Distinct();
        }


    }
}