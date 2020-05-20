using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using APEC.DOCS.Helpers.Extensions;

namespace APEC.DOCS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DefaultModelBinder.ResourceClassKey = "Global";

//            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
//            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeModelBinder());
        }
    }
}
