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

            IocConfig.RegisterDependencies();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

    }
        
}