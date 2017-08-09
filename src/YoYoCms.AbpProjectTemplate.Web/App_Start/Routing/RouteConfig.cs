using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace YoYoCms.AbpProjectTemplate.Web.Routing
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // 管理端项目路由
            routes.MapRoute(
                name: "ForntProj",
                url: "view/{*path}",
                defaults: new { controller = "Application", action = "Vue" },
                namespaces: new[] { "YoYoCms.AbpProjectTemplate.Web.Controllers" }
            );

            //ASP.NET Web API Route Config
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );




            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "YoYoCms.AbpProjectTemplate.Web.Controllers" }
            );
        }
    }
}
