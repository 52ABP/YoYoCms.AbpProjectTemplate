using System.Drawing;
using System.IO;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using LTM.Common.Drawing;
using Microsoft.AspNet.Identity;
using YoYoCms.AbpProjectTemplate.AppExtensions.AbpSessions;
using YoYoCms.AbpProjectTemplate.Configuration;

namespace YoYoCms.AbpProjectTemplate.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// Add your methods to this class common for all controllers.
    /// </summary>
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


        #region 验证码生成和验证
   

        /// <summary>
        /// 校验验证码信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool VerifyTheCaptcha(string code)
        {
            var sessionCaptcha = Session[AppSettings.UserManagement.UseCaptchaOnRegistration].ToString().ToLower();
            return sessionCaptcha == code.ToLower();
        }


 





        #endregion


    }
}