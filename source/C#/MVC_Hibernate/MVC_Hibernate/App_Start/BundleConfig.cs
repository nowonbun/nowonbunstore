using System.Web;
using System.Web.Optimization;

namespace MVC_Hibernate
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/metisMenu.min.js",
                      "~/Scripts/sb-admin-2.js",
                      "~/Scripts/common.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/common.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/metisMenu.min.css",
                      "~/Content/sb-admin-2.min.css"
                      ));
        }
    }
}
