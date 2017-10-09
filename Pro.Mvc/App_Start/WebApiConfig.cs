using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Pro.Mvc
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Routes.MapHttpRoute(
            name: "MemberAdd",
            routeTemplate: "api/sync/MemberAdd",
            defaults: new { id = 0, controller = "Sync", action = "MemberAdd" }
            );
            config.Routes.MapHttpRoute(
            name: "PaymentNotifyPostForm",
            routeTemplate: "api/sync/PaymentNotify",
            defaults: new { id = 0, controller = "Sync", action = "PaymentNotify" }
            );
            config.Routes.MapHttpRoute(
             name: "NotifyPostForm",
             routeTemplate: "api/credit/notify",
             defaults: new { id = 0, controller = "Credit", action = "Notify" }
             );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
