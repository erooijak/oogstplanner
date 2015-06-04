using System.Web.Optimization;

namespace Oogstplanner.Web
{
    static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Note: bootstrap.css and bootstrap-dialog.css cannot be minified properly by ASP.NET 
            //       and are included manually in _Layout.cshtml
            bundles.Add(new StyleBundle("~/Content/Stylesheets/bootstrap").Include(
                "~/Content/Stylesheets/bootstrap-navbar.css",
                "~/Content/Stylesheets/bootstrap-signin.css",
                "~/Content/Stylesheets/animate.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Stylesheets/bootstrap-profile").Include(
                "~/Content/Stylesheets/bootstrap-profile.css"
            ));

            bundles.Add(new StyleBundle("~/Content/Stylesheets/oogstplanner.responsive-squares").Include(
                "~/Content/Stylesheets/oogstplanner.styles.css",
                "~/Content/Stylesheets/oogstplanner.responsive-square-elements.css"
            ));

            bundles.Add(new StyleBundle("~/Content/Stylesheets/oogstplanner.sowing-and-harvesting").Include(
                "~/Content/Stylesheets/jquery.fullPage.css",
                "~/Content/Stylesheets/typeaheadjs.css"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/loadCSS").Include(
                "~/Scripts/loadCSS.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/Scripts/jquery-{version}.min.js",
                "~/Scripts/jquery-ui-{version}.min.js",
                "~/Scripts/jquery.ui.touch-punch.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/bootstrap-dialog.min.js",
                "~/Scripts/jquery.noty.packaged.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/fullPage").Include(
                "~/Scripts/jquery.easings.min.js",
                "~/Scripts/jquery.slimscroll.min.js",
                "~/Scripts/jquery.fullPage.js",
                "~/Scripts/oogstplanner.fullPage.js"));

            bundles.Add(new ScriptBundle("~/Scripts/oogstplanner").Include(
                "~/Scripts/oogstplanner.js",
                "~/Scripts/oogstplanner.utilities.js",
                "~/Scripts/oogstplanner.site-settings.js",
                "~/Scripts/oogstplanner.event-listeners.js",
                "~/Scripts/oogstplanner.notifications.js"));

            bundles.Add(new ScriptBundle("~/Scripts/oogstplanner.sowing-and-harvesting").Include(
                "~/Scripts/handlebars.min.js",
                "~/Scripts/typeahead.bundle.min.js",
                "~/Scripts/oogstplanner.drag-and-drop.js",
                "~/Scripts/oogstplanner.crops-suggestion-engine.js",
                "~/Scripts/oogstplanner.crop-selection-box.js"));
                
            BundleTable.EnableOptimizations = true;
        }
    }
}
