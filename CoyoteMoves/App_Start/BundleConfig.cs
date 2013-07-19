using System.Web;
using System.Web.Optimization;

namespace CoyoteMoves
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/Public/js/lib/modernizr.js",
                        "~/Public/js/lib/angular.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Public/css/Site.css",
                "~/Public/css/TodoList.css"));
        }
    }
}