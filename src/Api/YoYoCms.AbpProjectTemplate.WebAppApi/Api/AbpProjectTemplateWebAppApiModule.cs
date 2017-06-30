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
using YoYoCms.AbpProjectTemplate.AppExtensions.Attribute;

namespace YoYoCms.AbpProjectTemplate.WebAppApi.Api
{
    /// <summary>
    /// 在系统中进行WebApi的配置
    /// </summary>
    [DependsOn(typeof(AbpWebApiModule), typeof(AbpProjectTemplateApplicationModule))]
    public class AbpProjectTemplateWebAppApiModule : AbpModule
    {
       
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

           
            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(AbpProjectTemplateApplicationModule).Assembly, "yoyocms").ForMethods(
                    a =>
                    {
                        if (!a.Method.IsDefined(typeof(WebAppApiAttribute)))
                        {
                            a.DontCreate = true;
                        }
                    })
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
 
            ConfigureSwaggerUi();
        }

        private void ConfigureSwaggerUi()
        {
            Configuration.Modules.AbpWebApi().HttpConfiguration
                 .EnableSwagger("docs/{apiVersion}/apis", c =>
                 {
                     //webapp的API方法
                    c.SingleApiVersion("v1", "YoYoCms.AbpProjectTemplate.WebAppApi");
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var applicationFileName = "bin\\" + typeof(AbpProjectTemplateApplicationModule).Assembly.GetName().Name +
                                           ".XML";
                    var applicationFile = Path.Combine(baseDirectory, applicationFileName);
                    c.IncludeXmlComments(applicationFile);
                    var webapiFileName = "bin\\" + typeof(AbpProjectTemplateWebAppApiModule).Assembly.GetName().Name + ".XML";
                    var webapiFile = Path.Combine(baseDirectory, webapiFileName);
                    c.IncludeXmlComments(webapiFile);




                })
                .EnableSwaggerUi("docs/{*assetPath}", c =>
                {
                    c.InjectJavaScript(Assembly.GetAssembly(typeof(AbpProjectTemplateWebAppApiModule)), "YoYoCms.AbpProjectTemplate.WebAppApi.Api.Scripts.Swagger-Custom.js");
                });
        }
    }
}
