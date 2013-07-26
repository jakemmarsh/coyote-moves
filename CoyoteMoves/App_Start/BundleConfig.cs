using System.Web;
using System.Web.Optimization;

namespace CoyoteMoves
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/Public/js/lib/modernizr.js",
                        "~/Public/js/lib/angular.js",
                        "~/Public/js/lib/uiUtils.js",
                        "~/Public/js/lib/uiMap.js",
                        "~/Public/js/lib/jquery-1.10.2.js",
                        "~/Public/js/lib/bootstrap.js",
                        "~/Public/js/lib/angular-strap.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Public/js/controllers.js",
                        "~/Public/js/app.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Public/css/style.css",
                "~/Public/css/normalize.css"));
        }
    }
}