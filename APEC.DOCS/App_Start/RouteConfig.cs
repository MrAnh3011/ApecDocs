
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace APEC.DOCS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Default redirect to Admin/Home/Index
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{ssk}/{id}",
                defaults: new { controller = "Home", action = "Index", ssk = UrlParameter.Optional, id = UrlParameter.Optional },
                namespaces: new[] { "APEC.DOCS.Controllers" }
            );

            // Default redirect to Home/Index
            //            routes.MapRoute(
            //                name: "Default",
            //                url: "{controller}/{action}/{id}",
            //                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //                namespaces: new[] { "APEC.DOCS.Controllers" }
            //            );
        }
    }
}
