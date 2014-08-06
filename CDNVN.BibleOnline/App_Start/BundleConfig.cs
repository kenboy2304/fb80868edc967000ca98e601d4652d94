using System.Web;
using System.Web.Optimization;

namespace CDNVN.BibleOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/theme/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/theme/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/theme/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-select.min.js"));
            
            bundles.Add(new ScriptBundle("~/themes/app").Include(
                     "~/Scripts/themes/biblev1/app.js"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                     "~/Content/css/bootstrap-select.min.css",
                     "~/Content/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/system").Include(
                     "~/Content/css/reset.css",
                     "~/Content/css/icon-font.css"));


            bundles.Add(new StyleBundle("~/content/themes/biblev1/css").Include(     
                     "~/Content/themes/biblev1/style.css"));
        }
    }
}
