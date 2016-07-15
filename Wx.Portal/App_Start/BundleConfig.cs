using System.Web;
using System.Web.Optimization;

namespace Wx.Portal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/front/script").Include(
                        "~/front/assets/global/plugins/jquery.min.js",
"~/front/assets/global/plugins/bootstrap/js/bootstrap.js",
"~/front/assets/global/plugins/js.cookie.min.js",
"~/front/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.js",
"~/front/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.js",
"~/front/assets/global/plugins/jquery.blockui.min.js",
"~/front/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.js",
"~/front/assets/global/scripts/app.js",
"~/front/assets/layouts/layout4/scripts/layout.js",
"~/front/assets/layouts/layout4/scripts/demo.js",
"~/front/assets/layouts/global/scripts/quick-sidebar.js",


                        "~/front/scripts/linq.js",
                        "~/front/scripts/require.js",
                        "~/front/scripts/Vue.js"
                        ,
                        "~/front/scripts/bundle.js",

                        "~/front/scripts/text.js",
                        "~/front/scripts/page.js"
                        ,
                        "~/front/scripts/vue_pagination.js"
                        ,
                        "~/front/scripts/vue_resource.js"
                        ,
                        //"~/front/common/directives.js",
                "~/front/scripts/routes.js"




                        ));

            bundles.Add(new StyleBundle("~/front/css").Include(
                        "~/front/assets/global/plugins/font-awesome/css/font-awesome.css",
                        "~/front/assets/global/plugins/simple-line-icons/simple-line-icons.css",
                        "~/front/assets/global/plugins/bootstrap/css/bootstrap.css",

                        "~/front/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.css",
                        "~/front/assets/global/css/components-rounded.css",

                        //"~/front/assets/global/css/plugins.css",

                        "~/front/assets/layouts/layout4/css/layout.css",
                        "~/front/assets/layouts/layout4/css/themes/light.css",
                        "~/front/assets/layouts/layout4/css/custom.css"
                      ));

        }
    }
}
