using System.Web;
using System.Web.Optimization;

namespace CodeRumWebBlog
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jscore").Include(
                        "~/Resourse/client/js/jquery-3.3.1.min.js",
                        "~/Resourse/plugin/ajax/jquery.unobtrusive-ajax.js",
                        "~/Resourse/plugin/dododot/jquery.dotdotdot.js",
                        "~/Resourse/client/js/bootstrap.bundle.js",
                        "~/Resourse/client/js/bootstrap.min.js",
                        "~/Resourse/client/js/jquery.nice-select.min.js",
                        "~/Resourse/client/js/jquery-ui.min.js",
                        "~/Resourse/client/js/jquery.slicknav.js",
                        "~/Resourse/client/js/mixitup.min.js",
                        "~/Resourse/client/js/owl.carousel.min.js",
                        "~/Resourse/client/js/main.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jsadmin").Include(
                        "~/Resourse/admin/vendor/jquery/jquery.min.js",
                        "~/Resourse/plugin/ajax/jquery.unobtrusive-ajax.js",
                        "~/Resourse/admin/vendor/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Resourse/admin/vendor/jquery-easing/jquery.easing.min.js",
                        "~/Resourse/plugin/ckfinder/ckfinder.js",
                        "~/Resourse/plugin/ckeditor/ckeditor.js",
                        "~/Resourse/admin/js/sb-admin-2.js"
                        ));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new Bundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/csscore").Include(
                      "~/Resourse/client/css/bootstrap.min.css",
                      "~/Resourse/client/css/elegant-icons.css",
                      "~/Resourse/client/css/nice-select.css",
                      "~/Resourse/client/css/jquery-ui.min.css",
                      "~/Resourse/client/css/owl.carousel.min.css",
                      "~/Resourse/client/css/slicknav.min.css",
                      "~/Resourse/client/css/style.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/cssutils").Include(
                "~/Resourse/client/dist/ekko-lightbox.css",
                "~/Resourse/client/plugin/styles/hybrid.css"
                ));
            bundles.Add(new StyleBundle("~/bundles/toolip").Include(
                "~/Resourse/client/css/tooltip.css"
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
