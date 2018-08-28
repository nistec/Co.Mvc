using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Pro.Netcell.Web
{
    /*
    public ActionResult BreadCrumb()
    {
     Uri referrer = Request.UrlReferrer;
     if (referrer != null && referrer.IsRouteMatch("Catalog", "Category"))
     {
      string categoryName = referrer.GetRouteParameterValue("categoryName");
     }
    }
    */

    public static class UriExtensions
    {
        public static bool IsRouteMatch(this Uri uri, string controllerName, string actionName)
        {
            RouteInfo routeInfo = new RouteInfo(uri, HttpContext.Current.Request.ApplicationPath);
            return (routeInfo.RouteData.Values["controller"].ToString() == controllerName && routeInfo.RouteData.Values["action"].ToString() == actionName);
        }

        public static string GetRouteParameterValue(this Uri uri, string parameterName)
        {
            var routeInfo = new RouteInfo(uri, HttpContext.Current.Request.ApplicationPath);
            return routeInfo.RouteData.Values[parameterName] != null ? routeInfo.RouteData.Values[parameterName].ToString() : null;
        }
    }

        public class RouteInfo
        {
            public RouteInfo(Uri uri, string applicaionPath)
            {
                RouteData = RouteTable.Routes.GetRouteData(new InternalHttpContext(uri, applicaionPath));
            }

            public RouteData RouteData { get; private set; }

            private class InternalHttpContext : HttpContextBase
            {
                private readonly HttpRequestBase _request;

                public InternalHttpContext(Uri uri, string applicationPath)
                {
                    _request = new InternalRequestContext(uri, applicationPath);
                }

                public override HttpRequestBase Request
                {
                    get { return _request; }
                }
            }

            private class InternalRequestContext : HttpRequestBase
            {
                private readonly string _appRelativePath;
                private readonly string _pathInfo;

                public InternalRequestContext(Uri uri, string applicationPath)
                {
                    _pathInfo = "";
                    if (string.IsNullOrEmpty(applicationPath) || !uri.AbsolutePath.StartsWith(applicationPath, StringComparison.OrdinalIgnoreCase))
                    {
                        _appRelativePath = uri.AbsolutePath.Substring(applicationPath.Length);
                    }
                    else
                    {
                        _appRelativePath = uri.AbsolutePath;
                    }
                }

                public override string AppRelativeCurrentExecutionFilePath
                {
                    get { return string.Concat("~", _appRelativePath); }
                }

                public override string PathInfo
                {
                    get { return _pathInfo; }
                }
            }
        }
    
}
