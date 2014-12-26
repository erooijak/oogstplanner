using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Security;
using System.Web.Routing;
using WebMatrix.WebData;

using Zk.Models;

namespace Zk
{
    public class MvcApplication : HttpApplication
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

            routes.MapRoute (
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );

        }

        protected void Application_Start()
        {
            if (!WebSecurity.Initialized) 
            {
                WebSecurity.InitializeDatabaseConnection("ZkTestDatabaseConnection", "Users", "UserId", "Name", autoCreateTables: true);
            }

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

        }

    }
        
}