using Abp.Domain.Services;
using YoYoCms.AbpProjectTemplate.AppExtensions.AbpSessions;

namespace YoYoCms.AbpProjectTemplate
{
    public abstract class AbpProjectTemplateDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */
        public new IAbpSessionExtensions AbpSession { get; set; }

        protected AbpProjectTemplateDomainServiceBase()
        {
            LocalizationSourceName = AbpProjectTemplateConsts.LocalizationSourceName;
        }
    }
}
