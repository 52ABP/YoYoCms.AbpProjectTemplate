using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.IO;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.WebApi;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Security;
using Swashbuckle.Application;
using YoYoCms.AbpProjectTemplate.Web;
using YoYoCms.AbpProjectTemplate.WebApi;

namespace YoYoCms.AbpProjectTemplate.WebApp
{

    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(AbpZeroOwinModule),
        typeof(AbpProjectTemplateDataModule),
           typeof(AbpProjectTemplateWebApiModule),

        typeof(AbpProjectTemplateApplicationModule)

    )]
    public class AbpProjectTemplateWebAppModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();


       //     Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = WebUrlService.WebSiteRootAddress;


        }

    


        public override void Initialize()
        {
            //Dependency Injection
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient()
            );

            //Areas
            AreaRegistration.RegisterAllAreas();

            //Routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Bundling
            BundleTable.Bundles.IgnoreList.Clear();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

         

            //Automatically creates Web API controllers for all application services of the application
            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(AbpProjectTemplateApplicationModule).Assembly, "app")
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));


          //  ConfigureSwaggerUi();
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



                 })
                .EnableSwaggerUi("docs/{*assetPath}", c =>
                {
                    c.InjectJavaScript(Assembly.GetAssembly(typeof(AbpProjectTemplateWebAppModule)), "YoYoCms.AbpProjectTemplate.WebApp.Scripts.Swagger-Custom.js");
                });
        }
    }
}

 
