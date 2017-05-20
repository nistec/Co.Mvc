using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pro.Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           // routes.MapRoute(
           //  name: "StartupRegistry",
           //  url: "Registry/Signup/Mifkad",
           //  defaults: new { controller = "Registry", action = "Index", folder = "Mifkad" }
           //);


            /*
            routes.MapRoute(
               name: "RegistrySignup",
               url: "Registry/Signup/{folder}",
               defaults: new { controller = "Registry", action = "Signup", acc = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "RegistryCustom",
               url: "Registry/Custom/{folder}",
               defaults: new { controller = "Registry", action = "Custom", acc = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "RegistryCustomEx",
               url: "Registry/CustomEx/{folder}",
               defaults: new { controller = "Registry", action = "CustomEx", acc = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "RegistrySubscribe",
               url: "Registry/Subscribe/{folder}",
               defaults: new { controller = "Registry", action = "Subscribe", acc = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "RegistryIndex",
               url: "Registry/Index/{folder}",
               defaults: new { controller = "Registry", action = "Index", acc = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "RegistryConfirm",
               url: "Registry/Confirm/{folder}",
               defaults: new { controller = "Registry", action = "Confirm", acc = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "RegistryCredit",
               url: "Registry/Credit/{folder}",
               defaults: new { controller = "Registry", action = "Credit", acc = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "RegistryMsg",
               url: "Registry/Credit/{folder}",
               defaults: new { controller = "Registry", action = "Msg", acc = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "RegistryStaetment",
              url: "Registry/_Statement/{folder}",
              defaults: new { controller = "Registry", action = "_Statement", acc = UrlParameter.Optional }
          );
            */
          // routes.MapRoute(
          //    name: "RegistryAll",
          //    url: "Registry/{action}/{folder}",
          //    defaults: new { controller = "Registry", action = "{action}", acc = UrlParameter.Optional }
          //);

            //routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*robotstxt}", new { robotstxt = @"(.*/)?robots.txt(/.*)?" });

            routes.MapRoute(
             name: "DefaultRegistrySignup",
             url: "Registry/Signup/{folder}",
             defaults: new { controller = "Registry", action = "Signup", folder = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "DefaultRegistry",
              url: "Registry/{action}/{folder}",
              defaults: new { controller = "Registry", action = "Index", folder = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "DefaultPreview",
              url: "Preview/{action}/{folder}",
              defaults: new { controller = "Preview", action = "Index", folder = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Pro_Admin",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Manager", id = UrlParameter.Optional },
                namespaces: new string[] { "Pro.Mvc.Controllers.AdminController" }
            );
        }
    }
}