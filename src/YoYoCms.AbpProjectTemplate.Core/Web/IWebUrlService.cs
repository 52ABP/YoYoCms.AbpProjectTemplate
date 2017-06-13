namespace YoYoCms.AbpProjectTemplate.Web
{
    public interface IWebUrlService
    {
        string GetSiteRootAddress(string tenancyName = null);
    }
}
