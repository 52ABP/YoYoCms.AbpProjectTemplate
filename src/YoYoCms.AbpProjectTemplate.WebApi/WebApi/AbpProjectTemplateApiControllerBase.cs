using Abp.WebApi.Controllers;

namespace YoYoCms.AbpProjectTemplate.WebApi
{
    public abstract class AbpProjectTemplateApiControllerBase : AbpApiController
    {
        protected AbpProjectTemplateApiControllerBase()
        {
            LocalizationSourceName = AbpProjectTemplateConsts.LocalizationSourceName;
        }
    }
}