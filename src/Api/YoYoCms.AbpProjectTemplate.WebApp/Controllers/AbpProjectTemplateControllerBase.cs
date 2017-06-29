using Abp.IdentityFramework;
using Abp.Web.Mvc.Controllers;
using Microsoft.AspNet.Identity;
using YoYoCms.AbpProjectTemplate.AppExtensions.AbpSessions;

namespace YoYoCms.AbpProjectTemplate.WebApp.Controllers
{
   
    public abstract class AbpProjectTemplateControllerBase : AbpController
    {
        protected AbpProjectTemplateControllerBase()
        {
            LocalizationSourceName = AbpProjectTemplateConsts.LocalizationSourceName;
        }

        public new IAbpSessionExtensions AbpSession { get; set; }



        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}