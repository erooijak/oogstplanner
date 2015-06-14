using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Oogstplanner.Web.Utilities.ExtensionMethods
{
    /// <summary>
    /// Extension to add the active class to a link if we are on that page.
    /// Inspiration:  http://stackoverflow.com/questions/20410623/how-to-add-active-class-
    ///               to-html-actionlink-in-asp-net-mvc#29968637
    /// </summary>
    /// <example>
    /// <ul class="nav navbar-nav">
    ///     @Html.BootstrapActiveActionLink("About", "About", "Home")
    ///     @Html.BootstrapActiveActionLink("Contact", "Contact", "Home")
    /// </ul>
    /// </example>
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString BootstrapActiveActionLink(
            this HtmlHelper html, 
            string text, 
            string action, 
            string controller)
        {
            var context = html.ViewContext;
            if (context.Controller.ControllerContext.IsChildAction)
            {
                context = html.ViewContext.ParentActionViewContext;
            }
            var routeValues = context.RouteData.Values;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();

            var str = string.Format("<li role=\"presentation\"{0}>{1}</li>",
                currentAction.Equals(action, StringComparison.InvariantCulture) 
                    && currentController.Equals(controller, StringComparison.InvariantCulture) 
                ?" class=\"active\"" 
                : string.Empty, html.ActionLink(text, action, controller).ToHtmlString()
            );
            return new MvcHtmlString(str);
        }
    }
}
