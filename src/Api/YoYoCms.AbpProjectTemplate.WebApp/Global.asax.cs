using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;

namespace YoYoCms.AbpProjectTemplate.WebApp
{
    public class WebApiApplication : AbpWebApplication<AbpProjectTemplateWebAppModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            //Use UTC clock. Remove this to use local time for your application.
             
            //Log4Net configuration
            AbpBootstrapper.IocManager.IocContainer
                .AddFacility<LoggingFacility>(f => f.UseAbpLog4Net()
                    .WithConfig("log4net.config")
                );

            base.Application_Start(sender, e);
        }

       
    }
}
