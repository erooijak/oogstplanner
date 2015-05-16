using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Oogstplanner.Common;

using WebMatrix.WebData;

namespace Oogstplanner.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            if (!WebSecurity.Initialized) 
            {
                WebSecurity.InitializeDatabaseConnection(
                    ConfigurationHelper.ConnectionStringName, 
                    userTableName: "Users", 
                    userIdColumn: "UserId", 
                    userNameColumn: "Name", 
                    autoCreateTables: false);
            }

            IocConfig.RegisterDependencies();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }        
}
