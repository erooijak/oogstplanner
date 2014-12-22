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

        static SimpleMembershipInitializer _initializer;
        static object _initializerLock = new object();
        static bool _isInitialized;

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
            //LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
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

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            if (authCookie == null)
            {
                /*
                 There is no authentication cookie. User is not authenticated.

                 Instead of using asp.net built in redirect with the returnUrl querystring,
                 this will redirect user to login.aspx.
                 This is done so the URL looks cleaner ("/Account/Login" instead of
                 "/Account/Login?ReturnUrl=[gibberish]/").
                 The url of the current page will be checked to prevent the user from
                 being redirected to index.aspx again when redirected to index.aspx in
                 the first place.
                */
                string[] url = Request.RawUrl.Split('?');
                if (url[0] != "/Account/Login")
                {
                    Response.Redirect("/Account/Login");
                    return;
                }   
            }
        } 

	}

    public class SimpleMembershipInitializer
    {
        public SimpleMembershipInitializer()
        {
            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("ZkTestDatabaseConnection", "Users", "UserId", "Name", autoCreateTables: true);
        }
    }
}