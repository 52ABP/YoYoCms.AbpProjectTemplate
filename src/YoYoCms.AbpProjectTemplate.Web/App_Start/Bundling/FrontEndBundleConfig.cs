using System.Web.Optimization;

namespace YoYoCms.AbpProjectTemplate.Web.Bundling
{
    public static class FrontEndBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //LIBRARIES

            AddFrontendCssLibs(bundles, false);
            AddFrontendCssLibs(bundles, true);

            bundles.Add(
                new ScriptBundle("~/Bundles/Frontend/libs/js")
                    .Include(
                       
                        ScriptPaths.MomentJs,
                        ScriptPaths.MomentTimezoneJs,
                        ScriptPaths.Abp,
                        ScriptPaths.Abp_JQuery,
                        ScriptPaths.Abp_Toastr,
                        ScriptPaths.Abp_BlockUi,
                        ScriptPaths.Abp_SpinJs,
                        ScriptPaths.Abp_SweetAlert,
                        ScriptPaths.Abp_Moment
                    ).ForceOrdered()
                );

            //METRONIC

            AddFrontendCssMetronic(bundles, false);
            AddFrontendCssMetronic(bundles, true);

 
        }

        private static void AddFrontendCssLibs(BundleCollection bundles, bool isRTL)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/Frontend/libs/css" + (isRTL ? "RTL" : ""))
                    .Include(StylePaths.Simple_Line_Icons, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.FontAwesome, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.FamFamFamFlags, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(isRTL ? StylePaths.BootstrapRTL : StylePaths.Bootstrap, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.SweetAlert)
                    .Include(StylePaths.Toastr)
                    .ForceOrdered()
                );
        }

        private static void AddFrontendCssMetronic(BundleCollection bundles, bool isRTL)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/Frontend/metronic/css" + (isRTL ? "RTL" : ""))
                    .Include("~/metronic/assets/global/css/components" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/frontend/layout/css/style" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/frontend/pages/css/style-revolution-slider.css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/frontend/layout/css/style-responsive" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/frontend/layout/css/themes/red" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .ForceOrdered()
                );
        }
    }
}