using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pro.Mvc.Models
{
    public class CustomAuthorize : AuthorizeAttribute
    {

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                base.HandleUnauthorizedRequest(filterContext);

            }
            else
            {
                //logged and wihout the role to access it - redirect to the custom controller action
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new HttpUnauthorizedResult(); // Try this but i'm not sure
            filterContext.Result = new RedirectResult("~/Home/Unauthorized");
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    base.OnAuthorization(filterContext);
        //    if (filterContext.Cancel && filterContext.Result is HttpUnauthorizedResult)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(
        //          new RouteValueDictionary {
        //              { "clubShortName", filterContext.RouteData.Values[ "clubShortName" ] },
        //              { "controller", "Account" },
        //              { "action", "Login" },
        //              { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
        //            });
        //    }
        //}
    }
}