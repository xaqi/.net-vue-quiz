using System.Web;
using System.Web.Optimization;

namespace Wx.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/content/script").Include(
                        "~/content/front/scripts/linq.js",
                        "~/content/front/scripts/require.js",
                        "~/content/front/scripts/Vue.js"
                        ,
                        "~/content/front/scripts/bundle.js",

                        "~/content/front/scripts/text.js",
                        "~/content/front/scripts/page.js"
                        ,
                        "~/content/front/scripts/vue_resource.js"
                        ,
                        "~/content/front/common/vue_util.js",
                        "~/content/front/common/vue_focus.js",
                        "~/content/front/scripts/MintLoadmore.js",
                        "~/content/front/scripts/VueIndicator.js",
                        "~/content/front/scripts/routes.js",
                        "~/content/front/Scripts/vue_focus.js"

                        //"~/content/front/scripts/bundle.js"

                        ));

            bundles.Add(new StyleBundle("~/content/front/design/style/css").Include(
                      "~/content/front/design/bootstrap-3.3.5-dist/css/bootstrap.min.css",
                      "~/content/front/design/style/style.css",
                      "~/content/front/design/style/alert.css",
                        "~/content/front/scripts/MintLoadmore.css",
                        "~/content/front/scripts/VueIndicator.css",
                      "~/content/front/design/style/css.css"));

            bundles.Add(new StyleBundle("~/content/front/design/hero/style/css").Include(
                      "~/content/front/design/bootstrap-3.3.5-dist/css/bootstrap.min.css",
                      "~/content/front/design/hero/style/style.css",
                      "~/content/front/design/hero/style/alert.css",
                        "~/content/front/scripts/MintLoadmore.css",
                        "~/content/front/scripts/VueIndicator.css",
                      "~/content/front/design/hero/style/css.css"));
        }
    }
}
