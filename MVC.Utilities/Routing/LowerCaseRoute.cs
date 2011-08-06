using System.Web.Routing;

namespace MVC.Utilities.Routing
{
    /// <summary>
    /// Used with permission from Nick Berardi
    /// Original source: http://coderjournal.com/2008/03/force-mvc-route-url-lowercase/
    /// </summary>
    public class LowerCaseRoute : Route
    {
        public LowerCaseRoute(string url, IRouteHandler routeHandler) : base(url, routeHandler) { }
        public LowerCaseRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler) { }
        public LowerCaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : base(url, defaults, constraints, routeHandler) { }
        public LowerCaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) : base(url, defaults, constraints, dataTokens, routeHandler) { }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var path = base.GetVirtualPath(requestContext, values);
            if (path != null)
                path.VirtualPath = path.VirtualPath.ToLowerInvariant();
            return path;
        }
    }
}
