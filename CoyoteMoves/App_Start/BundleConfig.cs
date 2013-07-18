using System.Web;
using System.Web.Optimization;

namespace CoyoteMoves
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/todo").Include(
                "~/Public/js/app/todo.bindings.js",
                "~/Public/js/app/todo.datacontext.js",
                "~/Public/js/app/todo.model.js",
                "~/Public/js/app/todo.viewmodel.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Public/js/lib/modernizr"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Public/css/Site.css",
                "~/Public/css/TodoList.css"));
        }
    }
}