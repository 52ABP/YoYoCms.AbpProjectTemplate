  
// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:54. All Rights Reserved.
//<生成时间>2017-07-03T17:31:54</生成时间>

using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using YoYoCms.AbpProjectTemplate.Authorization;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs.Authorization
{
	/// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="SmsMessagelogAppPermissions"/> for all permission names.
    /// </summary>
    public class SmsMessagelogAppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
					      //在这里配置了SmsMessagelog 的权限。

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            

            var manager=pages.Children.FirstOrDefault(a=>a.Name== SmsMessagelogAppPermissions.smsmessManager) ??  pages.CreateChildPermission(SmsMessagelogAppPermissions.smsmessManager, new FixedLocalizableString("信息管理"));


            var smsMessagelog = manager.CreateChildPermission(SmsMessagelogAppPermissions.SmsMessagelog , L("SmsMessagelog"));
            smsMessagelog.CreateChildPermission(SmsMessagelogAppPermissions.SmsMessagelog_CreateSmsMessagelog, L("CreateSmsMessagelog"));
            smsMessagelog.CreateChildPermission(SmsMessagelogAppPermissions.SmsMessagelog_EditSmsMessagelog, L("EditSmsMessagelog"));           
            smsMessagelog.CreateChildPermission(SmsMessagelogAppPermissions. SmsMessagelog_DeleteSmsMessagelog, L("DeleteSmsMessagelog"));

            var languages = manager.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));






        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpProjectTemplateConsts.LocalizationSourceName);
        }
    }




}