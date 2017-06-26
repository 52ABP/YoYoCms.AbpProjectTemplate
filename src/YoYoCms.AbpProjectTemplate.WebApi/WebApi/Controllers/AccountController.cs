using System;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Authorization.Users;
using Abp.Web.Models;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using YoYoCms.AbpProjectTemplate.Authorization;
using YoYoCms.AbpProjectTemplate.Authorization.Users;
using YoYoCms.AbpProjectTemplate.MultiTenancy;
using YoYoCms.AbpProjectTemplate.WebApi.Models;

namespace YoYoCms.AbpProjectTemplate.WebApi.Controllers
{
    public class AccountController : AbpProjectTemplateApiControllerBase
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        private readonly IAuthenticationManager _authenticationManager;
        private readonly LogInManager _logInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;

        static AccountController()
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
        }

        public AccountController(
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            LogInManager logInManager, IAuthenticationManager authenticationManager)
        {
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
            _authenticationManager = authenticationManager;
        }

        /// <summary>
        /// 授权验证，申请token信息
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResponse> Authenticate(LoginModel loginModel)
        {
            var loginResult = await GetLoginResultAsync(
                loginModel.UsernameOrEmailAddress,
                loginModel.Password,
                loginModel.TenancyName
                );

            var ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

            return new AjaxResponse(OAuthBearerOptions.AccessTokenFormat.Protect(ticket));
        }


        [HttpGet]
        public AjaxResponse Logout()
        {
         
            _authenticationManager.SignOut();
            return new AjaxResponse();
          
        }



        /// <summary>
        /// 获取登陆信息返回的结果
        /// </summary>
        /// <param name="usernameOrEmailAddress"></param>
        /// <param name="password"></param>
        /// <param name="tenancyName"></param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }
    }
}
