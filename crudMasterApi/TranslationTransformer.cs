using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace CrudMasterApi
{
    public class TranslationTransformer : DynamicRouteValueTransformer
    {

        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            if (!values.ContainsKey("options") || !values.ContainsKey("controller") || !values.ContainsKey("action"))
            {
                httpContext.Request.QueryString = new QueryString("?CurrentPageNumber=1&numOfRowsPerPage=2");
                var newVal = new RouteValueDictionary();
                newVal.Add("api","api");
                newVal.Add("Controller","School");
                return new ValueTask<RouteValueDictionary>(newVal);
            }
                
            
            return new ValueTask<RouteValueDictionary>(values);

        }
    }
}
