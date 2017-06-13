using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Views;

namespace YoYoCms.AbpProjectTemplate.Web.Views
{
    public abstract class AbpProjectTemplateWebViewPageBase : AbpProjectTemplateWebViewPageBase<dynamic>
    {
       
    }

    public abstract class AbpProjectTemplateWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        public IAbpSession AbpSession { get; private set; }
        
        protected AbpProjectTemplateWebViewPageBase()
        {
            AbpSession = IocManager.Instance.Resolve<IAbpSession>();
            LocalizationSourceName = AbpProjectTemplateConsts.LocalizationSourceName;
        }
    }
}