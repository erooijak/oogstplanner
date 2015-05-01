using System.Web.Mvc;

namespace Zk.Utilities.ExtensionMethods
{
    public static class UrlHelperExtension
    {
        /// <summary>
        ///     Return a view path based on an action name and controller name.
        ///     See http://stackoverflow.com/questions/21089917/
        ///         how-to-return-partial-view-of-another-controller-by-controller#21093519
        /// </summary>
        /// <param name="url">Context for extension method</param>
        /// <param name="action">Action name</param>
        /// <param name="controller">Controller name</param>
        /// <returns>A string in the form "~/views/{controller}/{action}.cshtml</returns>
        public static string View(this UrlHelper url, string action, string controller)
        {
            return string.Format("~/Views/{1}/{0}.cshtml", action, controller);
        }
    }
}