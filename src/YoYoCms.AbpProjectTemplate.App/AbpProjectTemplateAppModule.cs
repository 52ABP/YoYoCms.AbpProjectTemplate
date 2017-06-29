using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.WebApi;
using Swashbuckle.Application;

namespace YoYoCms.AbpProjectTemplate.App
{
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(AbpProjectTemplateDataModule),
        typeof(AbpProjectTemplateApplicationModule),
        typeof(AbpProjectTemplateDataModule),
        typeof(AbpProjectTemplateApplicationModule),
        typeof(AbpWebApiModule))]
    public class AbpProjectTemplateAppModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Automatically creates Web API controllers for all application services of the application
            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(AbpProjectTemplateApplicationModule).Assembly, "app")
                .Build();
            // Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigureSwaggerUi();
        }

        private void ConfigureSwaggerUi()
        {
            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger("docs/{apiVersion}/apis", c =>
                {
                    c.SingleApiVersion("v1", "YoYoCms.AbpProjectTemplate.App");
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var applicationFileName = "bin\\" +
                                              typeof(AbpProjectTemplateApplicationModule).Assembly.GetName().Name +
                                              ".XML";
                    var applicationFile = Path.Combine(baseDirectory, applicationFileName);
                    c.IncludeXmlComments(applicationFile);
                })
                .EnableSwaggerUi("docs/{*assetPath}",
                    c =>
                    {
                        c.InjectJavaScript(Assembly.GetAssembly(typeof(AbpProjectTemplateAppModule)),
                            "YoYoCms.AbpProjectTemplate.App.Scripts.Swagger-Custom.js");
                    });
        }
    }
}