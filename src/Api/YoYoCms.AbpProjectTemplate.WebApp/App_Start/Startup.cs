using System;
using Abp.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using YoYoCms.AbpProjectTemplate.WebApp;
using YoYoCms.AbpProjectTemplate.WebAppApi.Api.Controllers;

[assembly: OwinStartup(typeof(Startup))]

namespace YoYoCms.AbpProjectTemplate.WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();

            app.RegisterDataProtectionProvider();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);
 
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //You can remove these lines if you don't like to use two factor auth (while it has no problem if you don't remove)
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
 

            //Enable it to use HangFire dashboard (uncomment only if it's enabled in AbpProjectTemplateWebModule)
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard) }
            //});
        }
 
  
    }
}