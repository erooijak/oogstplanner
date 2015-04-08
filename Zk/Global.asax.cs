using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using WebMatrix.WebData;

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

            // Insecure fix for anti forgery token exception.
            // See stackoverflow.com/questions/2206595/how-do-i-solve-an-antiforgerytoken-exception-that-occurs-after-an-iisreset-in-my#20421618
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

        }

    }
        
}