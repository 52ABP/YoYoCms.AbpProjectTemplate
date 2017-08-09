using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Controllers.Results;
using LTM.Common.Drawing;
using YoYoCms.AbpProjectTemplate.Configuration;

namespace YoYoCms.AbpProjectTemplate.Web.Controllers
{
    public class CaptchaController : AbpProjectTemplateControllerBase
    {

        /// <summary>
        /// 生成图形验证码信息
        /// </summary>
        /// <param name="captchaKey">图形验证码的key</param>
        /// <returns></returns>
        public FileContentResult CreateGraphValidateCode(string captchaKey = AppSettings.UserManagement.UseCaptchaOnRegistration)
        {
            //UseCaptchaOnRegistration
            var length = captchaKey.IndexOf('?');
            var key = length <= 0 ? captchaKey : captchaKey.Substring(0, captchaKey.IndexOf('?'));
            var vCode = new ValidateCoder
            {
                FontSize = 20,
                FontWidth = 14,
                BgColor = Color.Cornsilk,
                RandomColor = true
            };
            var code = vCode.GetCode(4);
            Session[key] = code;
            var bmp = vCode.CreateImage(code, ValidateCodeType.Number);
            var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return File(ms.ToArray(), @"image/png");

        }
        /// <summary>
        /// 注册时检查用户输入的验证码是否正确
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public JsonResult CheckRegisterVailCodeByJsonResult(EntityDto<string> input)
        {
            if (input.Id.IsNullOrWhiteSpace())
            {
                throw new UserFriendlyException(L("CaptchaCanNotBeEmpty"));
            }

            var result = VerifyTheCaptcha(input.Id);
            if (result)
            {
                return Json(true);
            }
            throw new UserFriendlyException(L("IncorrectCaptchaAnswer"));
        }


    }
}