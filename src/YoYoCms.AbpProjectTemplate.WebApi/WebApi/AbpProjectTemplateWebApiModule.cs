using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Swashbuckle.Application;

namespace YoYoCms.AbpProjectTemplate.WebApi
{
    /// <summary>
    /// Web API layer of the application.
    /// </summary>
    [DependsOn(typeof(AbpWebApiModule), typeof(AbpProjectTemplateApplicationModule))]
    public class AbpProjectTemplateWebApiModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false;
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Automatically creates Web API controllers for all application services of the application
            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(AbpProjectTemplateApplicationModule).Assembly, "yoyocms")
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
 
            ConfigureSwaggerUi();
        }

        private void ConfigureSwaggerUi()
        {
            Configuration.Modules.AbpWebApi().HttpConfiguration
                 .EnableSwagger("docs/{apiVersion}/apis", c =>
                 {
                    c.SingleApiVersion("v1", "YoYoCms.AbpProjectTemplate.WebApi");
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var applicationFileName = "bin\\" + typeof(AbpProjectTemplateApplicationModule).Assembly.GetName().Name +
                                           ".XML";
                    var applicationFile = Path.Combine(baseDirectory, applicationFileName);
                    c.IncludeXmlComments(applicationFile);
                    var webapiFileName = "bin\\" + typeof(AbpProjectTemplateWebApiModule).Assembly.GetName().Name + ".XML";
                    var webapiFile = Path.Combine(baseDirectory, webapiFileName);
                    c.IncludeXmlComments(webapiFile);




                })
                .EnableSwaggerUi("docs/{*assetPath}", c =>
                {
                    c.InjectJavaScript(Assembly.GetAssembly(typeof(AbpProjectTemplateWebApiModule)), "YoYoCms.AbpProjectTemplate.WebApi.Scripts.Swagger-Custom.js");
                });
        }
    }
}
