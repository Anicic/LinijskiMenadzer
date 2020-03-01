using System.Web;
using System.Web.Optimization;

namespace WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/monthly").Include(
                    "~/Content/monthly.css"));

            bundles.Add(new ScriptBundle("~/bundles/monthly").Include(
                      "~/Scripts/monthly/monthly.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                    "~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/Scripts/jtable-scripts").Include(
                  "~/Scripts/jquery-ui-1.12.1.js",
                  "~/Scripts/jtable/jquery.jtable.js"
                ));

            bundles.Add(new StyleBundle("~/Content/jtable-css").Include(
           "~/Content/themes/base/jquery-ui.min.css",
           "~/Scripts/jtable/themes/lightcolor/blue/jtable.css"));

            bundles.Add(new ScriptBundle("~/bundles/ecoone").Include(
                      "~/Scripts/ecoone/core.min.js",
                      "~/Scripts/ecoone/nicescroll.min.js",
                      "~/Scripts/ecoone/moment.min.js",
                      "~/Scripts/ecoone/daterangepicker.js",
                      "~/Scripts/ecoone/app.min.js",
                      "~/Scripts/ecoone/datepicker.js",
                      "~/Scripts/ecoone/sidebar.js"));

            bundles.Add(new StyleBundle("~/Content/ecoone").Include(
                       "~/Content/econe.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/Site.css"));


            bundles.Add(new ScriptBundle("~/bundles/summernote").Include(
                     "~/Scripts/summernote/summernote.min.js"));

            bundles.Add(new StyleBundle("~/Content/summernote").Include(
                     "~/Scripts/summernote/summernote.css"));

            bundles.Add(new ScriptBundle("~/bundles/flatpickr").Include(
                     "~/Scripts/App/Flatpickr.js"));

        }
    }
}
