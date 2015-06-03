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
                
            BundleTable.EnableOptimizations = true;
        }
    }
}
