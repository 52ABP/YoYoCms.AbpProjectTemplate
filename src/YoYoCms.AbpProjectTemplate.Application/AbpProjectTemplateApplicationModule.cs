using System.Reflection;
using Abp.AutoMapper;
using Abp.Localization;
using Abp.Modules;
using YoYoCms.AbpProjectTemplate.Authorization;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs.Authorization;

namespace YoYoCms.AbpProjectTemplate
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(typeof(AbpProjectTemplateCoreModule))]
    public class AbpProjectTemplateApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
            Configuration.Authorization.Providers.Add<SmsMessagelogAppAuthorizationProvider>();


            //Adding custom AutoMapper mappings
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                CustomDtoMapper.CreateMappings(mapper);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
