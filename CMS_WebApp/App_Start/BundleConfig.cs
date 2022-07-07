using System.Web;
using System.Web.Optimization;

namespace CMS_WebApp
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/plugins/jQuery/jquery-2.2.3.min.js",
                        "~/Content/bootstrap/js/bootstrap.min.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",                        
                        "~/Content/plugins/datatables/jquery.dataTables.min.js",
                        "~/Content/plugins/datatables/dataTables.bootstrap.min.js",
                        "~/Content/plugins/slimScroll/jquery.slimscroll.min.js",
                        "~/Content/plugins/fastclick/fastclick.js",
                        "~/Content/dist/js/app.min.js",
                        "~/Content/dist/js/demo.js",
                        "~/Content/dist/js/demo.js",
                        "~/Content/plugins/datepicker/bootstrap-datepicker.js",
                        "~/Scripts/readyPicker.js",
                        "~/Content/plugins/select2/select2.full.min.js",
                        "~/Scripts/admins/extend.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap/css/bootstrap.min.css",
                    "~/Content/dist/css/AdminLTE.min.css",
                    "~/Content/plugins/datatables/dataTables.bootstrap.css",
                    "~/Content/dist/css/skins/_all-skins.min.css",
                    "~/Content/plugins/datepicker/datepicker3.css",
                    "~/Content/loader.css",
                    "~/Content/plugins/select2/select2.min.css"));
        }
    }
}
