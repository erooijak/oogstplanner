using System.Web.Mvc;
using System.Web.Routing;

namespace Oogstplanner.Web
{
    static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Welcome",
                url: "welkom",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "SowingAndHarvesting",
                url: "zaaienenoogsten",
                defaults: new { controller = "Home", action = "SowingAndHarvesting" }
            );

            routes.MapRoute(
                name: "404",
                url: "404",
                defaults: new { controller = "Error", action = "NotFound" }
            );

            routes.MapRoute(
                name: "Crops",
                url: "gewassen",
                defaults: new { controller = "Crop", action = "Index" }
            );

            routes.MapRoute(
                name: "CropsJson",
                url: "gewassen/json",
                defaults: new { controller = "Crop", action = "All" }
            );

            routes.MapRoute(
                name: "YearCalendar",
                url: "zaaikalender",
                defaults: new { controller = "Calendar", action = "Year" }
            );
                
            routes.MapRoute(
                name: "UpdateMonth",
                url: "zaaikalender/update",
                defaults: new { controller = "Calendar", action = "UpdateMonth" }
            );

            routes.MapRoute(
                name: "RemoveFarmingAction",
                url: "zaaikalender/verwijder",
                defaults: new { controller = "Calendar", action = "RemoveFarmingAction" }
            );

            routes.MapRoute(
                name: "AddFarmingAction",
                url: "zaaikalender/toevoegen",
                defaults: new {  controller = "Calendar", action = "AddFarmingAction" }
            );

            routes.MapRoute(
                name: "GetMonthsWithAction",
                url: "zaaikalender/actievemaanden",
                defaults: new { controller = "Calendar", action = "GetMonthsWithAction" }
            );

            routes.MapRoute(
                name: "Month",
                url: "zaaikalender/{month}",
                defaults: new { controller = "Calendar", action = "Month", month = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LoginOrRegisterModal",
                url: "account/inloggenofregistreren",
                defaults: new { controller = "Account", action = "LoginOrRegisterModal" }
            );

            routes.MapRoute(
                name: "Login",
                url: "account/inloggen",
                defaults: new {  controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Register",
                url: "account/registreren",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "LogOff",
                url: "account/uitloggen",
                defaults: new { controller = "Account", action = "LogOff" }
            );

            routes.MapRoute(
                name: "LostPassword",
                url: "account/wachtwoordvergeten",
                defaults: new { controller = "Account", action = "LostPassword" }
            );

            routes.MapRoute(
                name: "ResetPassword",
                url: "account/wachtwoordreset",
                defaults: new { controller = "Account", action = "ResetPassword" }
            );

            routes.MapRoute(
                name: "ResetPasswordToken",
                url: "account/wachtwoordreset/{token}",
                defaults: new { controller = "Account", action = "ResetPassword", token = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AccountInfo",
                url: "account",
                defaults: new {  controller = "Account", action = "Info" }
            );

            routes.MapRoute(
                name: "AccountInfoUser",
                url: "account/{userName}",
                defaults: new { controller = "Account", action = "UserInfo", userName = UrlParameter.Optional }
            );

            routes.MapRoute (
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );
        }
    }        
}
