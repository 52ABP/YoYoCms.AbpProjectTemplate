using System.Web.Optimization;
using YoYoCms.AbpProjectTemplate.Web.Bundling;

namespace YoYoCms.AbpProjectTemplate.Web.AppStartup
{
    public static class AppBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //LIBRARIES

            AddAppCssLibs(bundles, isRTL: false);
            AddAppCssLibs(bundles, isRTL: true);

            bundles.Add(
                new ScriptBundle("~/Bundles/App/libs/js")
                    .Include(
                        ScriptPaths.Json2,
                        ScriptPaths.JQuery,
                        ScriptPaths.JQuery_Migrate,
                        ScriptPaths.Bootstrap,
                        ScriptPaths.Bootstrap_Hover_Dropdown,
                        ScriptPaths.JQuery_Slimscroll,
                        ScriptPaths.JQuery_BlockUi,
                        ScriptPaths.Js_Cookie,
                        ScriptPaths.JQuery_Uniform,
                        ScriptPaths.SignalR,
                        ScriptPaths.LocalForage,
                        ScriptPaths.Morris,
                        ScriptPaths.Morris_Raphael,
                        ScriptPaths.JQuery_Sparkline,
                        ScriptPaths.JQuery_Color,
                        ScriptPaths.JQuery_Jcrop,
                        ScriptPaths.JQuery_Timeago,
                        ScriptPaths.JsTree,
                        ScriptPaths.Bootstrap_Switch,
                        ScriptPaths.SpinJs,
                        ScriptPaths.SpinJs_JQuery,
                        ScriptPaths.SweetAlert,
                        ScriptPaths.Toastr,
                        ScriptPaths.MomentJs,
                        ScriptPaths.MomentTimezoneJs,
                        ScriptPaths.Bootstrap_DateRangePicker,
                        ScriptPaths.Bootstrap_Select,
                        ScriptPaths.Underscore,
                        ScriptPaths.Angular,
                        ScriptPaths.Angular_Sanitize,
                        ScriptPaths.Angular_Touch,
                        ScriptPaths.Angular_Ui_Router,
                        ScriptPaths.Angular_Ui_Utils,
                        ScriptPaths.Angular_Ui_Bootstrap_Tpls,
                        ScriptPaths.Angular_Ui_Grid,
                        ScriptPaths.Angular_OcLazyLoad,
                        ScriptPaths.Angular_File_Upload,
                        ScriptPaths.Angular_DateRangePicker,
                        ScriptPaths.Angular_Moment,
                        ScriptPaths.Angular_Bootstrap_Switch,
                        ScriptPaths.Abp,
                        ScriptPaths.Abp_JQuery,
                        ScriptPaths.Abp_Toastr,
                        ScriptPaths.Abp_BlockUi,
                        ScriptPaths.Abp_SpinJs,
                        ScriptPaths.Abp_SweetAlert,
                        ScriptPaths.Abp_Moment,
                        ScriptPaths.Abp_Angular
                    ).ForceOrdered()
                );

            //METRONIC

            AddAppMetronicCss(bundles, isRTL: false);
            AddAppMetronicCss(bundles, isRTL: true);

            bundles.Add(
              new ScriptBundle("~/Bundles/App/metronic/js")
                  .Include(
                      "~/metronic/assets/global/scripts/app.js",
                      "~/metronic/assets/admin/layout4/scripts/layout.js",
                      "~/metronic/assets/layouts/global/scripts/quick-sidebar.js"
                  ).ForceOrdered()
              );

            //APPLICATION

           

         
        }

        private static void AddAppCssLibs(BundleCollection bundles, bool isRTL)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/App/libs/css" + (isRTL ? "RTL" : ""))
                    .Include(StylePaths.FontAwesome, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Simple_Line_Icons, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.FamFamFamFlags, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(isRTL ? StylePaths.BootstrapRTL : StylePaths.Bootstrap, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.JQuery_Uniform, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Morris)
                    .Include(StylePaths.JsTree, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.SweetAlert)
                    .Include(StylePaths.Toastr)
                    .Include(StylePaths.Angular_Ui_Grid, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Bootstrap_DateRangePicker)
                    .Include(StylePaths.Bootstrap_Select)
                    .Include(StylePaths.Bootstrap_Switch)
                    .Include(StylePaths.JQuery_Jcrop)
                    .ForceOrdered()
                );
        }

        private static void AddAppMetronicCss(BundleCollection bundles, bool isRTL)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/App/metronic/css" + (isRTL ? "RTL" : ""))
                    .Include("~/metronic/assets/global/css/components-md" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/global/css/plugins-md" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/admin/layout4/css/layout" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/admin/layout4/css/themes/light" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .ForceOrdered()
                );
        }
    }
}
