using Abp.Domain.Services;

namespace YoYoCms.AbpProjectTemplate
{
    public abstract class AbpProjectTemplateDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected AbpProjectTemplateDomainServiceBase()
        {
            LocalizationSourceName = AbpProjectTemplateConsts.LocalizationSourceName;
        }
    }
}
