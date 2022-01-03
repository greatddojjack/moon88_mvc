using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace moon_album
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{category_id}/{id}",
                defaults: new { controller = "Home", action = "Index", category_id = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Admin",
                url: "{controller}/{action}/{category_id}/{id}",
                defaults: new { controller = "Admin", action = "Index", category_id = UrlParameter.Optional, id = UrlParameter.Optional }
            );
        }
    }
}
