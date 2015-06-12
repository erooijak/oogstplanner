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
                name: "LikeCalendar",
                url: "zaaikalender/like",
                defaults: new { controller = "Friends", action = "Like" }
            );

            // Maps to same controller and action as /zaaikalender url
            routes.MapRoute(
                name: "CalendarOfUser",
                url: "zaaikalender/gebruiker",
                defaults: new { controller = "Calendar", action = "Year" }
            );

            routes.MapRoute(
                name: "CalendarLikesCount",
                url: "zaaikalender/{calendarId}/aantal-likes",
                defaults: new { controller = "Friends", action = "GetLikesCount" }
            );

            routes.MapRoute(
                name: "CalendarLikesUsers",
                url: "zaaikalender/{calendarId}/gebruikers-die-liken",
                defaults: new { controller = "Friends", action = "GetLikesUserNames" }
            );

            routes.MapRoute(
                name: "Month",
                url: "zaaikalender/{month}",
                defaults: new { controller = "Calendar", action = "Month" }
            );

            routes.MapRoute(
                name: "LoginOrRegisterModal",
                url: "gebruiker/inloggenofregistreren",
                defaults: new { controller = "Account", action = "LoginOrRegisterModal" }
            );

            routes.MapRoute(
                name: "Login",
                url: "gebruiker/inloggen",
                defaults: new {  controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Register",
                url: "gebruiker/registreren",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "LogOff",
                url: "gebruiker/uitloggen",
                defaults: new { controller = "Account", action = "LogOff" }
            );

            routes.MapRoute(
                name: "LostPassword",
                url: "gebruiker/wachtwoordvergeten",
                defaults: new { controller = "Account", action = "LostPassword" }
            );

            routes.MapRoute(
                name: "ResetPassword",
                url: "gebruiker/wachtwoordreset",
                defaults: new { controller = "Account", action = "ResetPassword" }
            );

            routes.MapRoute(
                name: "ResetPasswordToken",
                url: "gebruiker/wachtwoordreset/{token}",
                defaults: new { controller = "Account", action = "ResetPassword" }
            );

            routes.MapRoute(
                name: "AccountInfo",
                url: "gebruiker",
                defaults: new {  controller = "Account", action = "Info" }
            );

            routes.MapRoute(
                name: "CalendarOfOtherUser",
                url: "gebruiker/{userName}/zaaikalender",
                defaults: new { controller = "Calendar", action = "YearForUser" }
            );

            routes.MapRoute(
                name: "AccountInfoUser",
                url: "gebruiker/{userName}",
                defaults: new { controller = "Account", action = "UserInfo" }
            );

            routes.MapRoute (
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );
        }
    }        
}
