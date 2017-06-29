using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Castle.Logging.Log4Net;
using Abp.Timing;
using Abp.Web;
using Castle.Facilities.Logging;

namespace YoYoCms.AbpProjectTemplate.App
{
    public class MvcApplication : AbpWebApplication<AbpProjectTemplateAppModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );

            base.Application_Start(sender, e);
        }
        private static readonly DateTime CacheExpireDate = new DateTime(2000, 1, 1);

        private void DisableClientCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(CacheExpireDate);
            Response.Cache.SetNoStore();
        }
    }
}
