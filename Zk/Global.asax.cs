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
            
        /* Return to cleaner URL, cannot be used with external logins */
        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = Context.Request.Cookies[cookieName];
            if (authCookie == null)
            {
                /*
                 There is no authentication cookie. User is not authenticated.

                 Instead of using asp.net built in redirect with the returnUrl querystring,
                 this will redirect user to login.aspx.
                 The url of the current page will be checked to prevent the user from
                 being redirected to index.cshtml again when redirected to index.cshtml in
                 the first place.
                */
                var url = Request.RawUrl.Split('?')[0];
                if (url != "/Account/Login")
                {
                    Response.Redirect("/Account/Login");
                    return;
                }   
            }
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