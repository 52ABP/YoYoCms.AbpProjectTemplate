using Abp.Dependency;
using Abp.Web.Mvc.Views;
using YoYoCms.AbpProjectTemplate.AppExtensions.AbpSessions;

namespace YoYoCms.AbpProjectTemplate.Web.Views
{
    public abstract class AbpProjectTemplateWebViewPageBase : AbpProjectTemplateWebViewPageBase<dynamic>
    {
       
    }

    public abstract class AbpProjectTemplateWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        public new IAbpSessionExtensions AbpSession { get; private set; }

        protected AbpProjectTemplateWebViewPageBase()
        {
            AbpSession = IocManager.Instance.Resolve<IAbpSessionExtensions>();
            LocalizationSourceName = AbpProjectTemplateConsts.LocalizationSourceName;
        }
    }
}