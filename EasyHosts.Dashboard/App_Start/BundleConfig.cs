using System.Web;
using System.Web.Optimization;

namespace EasyHosts.Dashboard
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //JQUERY
            bundles.Add(new ScriptBundle("~/Jquery/script").Include(
                        "~/Scripts/Jquery/jquery-{version}.js"));

            //OVERLAY SCROLL BAR CSS
            bundles.Add(new StyleBundle("~/Content/Overlayscrollbars").Include(
                      "~/Content/Overlayscrollbars/OverlayScrollbars.min.css"));

            //OTHERS CSS
            bundles.Add(new StyleBundle("~/Content/Css").Include(
                      "~/Content/Css/theme-font-styles.css",
                      "~/Content/Css/theme.min.css",
                      "~/Content/Css/user.min.css"));

            //PAGED LIST CSS
            bundles.Add(new StyleBundle("~/Content/PagedList").Include(
                      "~/Content/PageList/PagedList.css"));

            //CONFIG JS
            bundles.Add(new ScriptBundle("~/Config/script").Include(
                      "~/Scripts/Js/config.js"));

            //OVERLAY SCROLL BAR JS
            bundles.Add(new ScriptBundle("~/Overlayscrollbars/script").Include(
                      "~/Scripts/Overlayscrollbars/OverlayScrollbars.min.js"));

            //POPPER JS
            bundles.Add(new ScriptBundle("~/Popper/script").Include(
                      "~/Scripts/Popper/popper.min.js"));

            //BOOTSTRAP JS
            bundles.Add(new Bundle("~/Bootstrap/script").Include(
                      "~/Scripts/Bootstrap/bootstrap.min.js"));

            //ANCHOR JS
            bundles.Add(new ScriptBundle("~/Anchorjs/script").Include(
                      "~/Scripts/Anchorjs/anchor.min.js"));

            //IS JS
            bundles.Add(new ScriptBundle("~/Is/script").Include(
                      "~/Scripts/Is/is.min.js"));

            //ECHARTS JS
            bundles.Add(new ScriptBundle("~/Echarts/script").Include(
                      "~/Scripts/Echarts/echarts.min.js"));

            //FONTAWESOME JS
            bundles.Add(new ScriptBundle("~/Fontawesome/script").Include(
                      "~/Scripts/Fontawesome/all.min.js"));

            //LODASH JS
            bundles.Add(new ScriptBundle("~/Lodash/script").Include(
                      "~/Scripts/Lodash/lodash.min.js"));

            //POLYFILL JS
            bundles.Add(new ScriptBundle("~/Polyfill/script").Include(
                      "~/https://polyfill.io/v3/polyfill.min.js?features=window.scroll"));

            //LIST JS
            bundles.Add(new ScriptBundle("~/List.js/script").Include(
                      "~/Scripts/List.js/list.min.js"));

            //COUNTUP JS
            bundles.Add(new ScriptBundle("~/CountUp/script").Include(
                      "~/Scripts/CountUp/countUp.umd.js"));

            //OTHER JS
            bundles.Add(new ScriptBundle("~/Js/script").Include(
                      "~/Scripts/Js/theme.js"));

        }
    }
}
