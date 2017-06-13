using System.Configuration;
using Abp.Dependency;
using Abp.Extensions;

namespace YoYoCms.AbpProjectTemplate.Web
{
    public class WebUrlService : IWebUrlService, ITransientDependency
    {
        public const string TenancyNamePlaceHolder = "{TENANCY_NAME}";

        public static string WebSiteRootAddress => ConfigurationManager.AppSettings["WebSiteRootAddress"].EnsureEndsWith('/');

        public string GetSiteRootAddress(string tenancyName = null)
        {
            var siteRootFormat = WebSiteRootAddress;

            if (!siteRootFormat.Contains(TenancyNamePlaceHolder))
            {
                return siteRootFormat;
            }

            if (siteRootFormat.Contains(TenancyNamePlaceHolder + "."))
            {
                siteRootFormat = siteRootFormat.Replace(TenancyNamePlaceHolder + ".", TenancyNamePlaceHolder);
            }

            if (tenancyName.IsNullOrEmpty())
            {
                return siteRootFormat.Replace(TenancyNamePlaceHolder, "");
            }

            return siteRootFormat.Replace(TenancyNamePlaceHolder, tenancyName + ".");
        }
    }
}