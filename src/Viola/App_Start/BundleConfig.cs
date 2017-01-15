using System.Web;
using System.Web.Optimization;

namespace Viola
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // js
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                        "~/Scripts/main.js"));


            // css
            bundles.Add(new StyleBundle("~/Content/main").Include(
                      "~/Content/main.css"));


            

            // select2 js
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                        "~/Scripts/select2.min.js",
                        "~/Scripts/select2.tr.js"));
            // select2 css
            bundles.Add(new StyleBundle("~/Content/select2").Include(
                      "~/Content/select2.min.css"));


            // datetimepicker js
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                        "~/Scripts/moment.min.js",
                        "~/Scripts/moment.tr.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js"));
            // datetimepicker css
            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
                      "~/Content/bootstrap-datetimepicker.min.css"));


            // gridmvc js
            bundles.Add(new ScriptBundle("~/bundles/gridmvc").Include(
                        "~/Scripts/gridmvc.min.js"));
            // gridmvs css
            bundles.Add(new StyleBundle("~/Content/gridmvc").Include(
                      "~/Content/gridmvc.css"));
        }
    }
}
