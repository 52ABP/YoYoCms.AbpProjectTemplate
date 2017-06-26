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
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;

        private readonly IAuthenticationManager _authenticationManager;
        private readonly LogInManager _logInManager;

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

        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; }

        /// <summary>
        ///     授权验证，申请token信息
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
            var expiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = expiresUtc;

            var timeSpan = expiresUtc - DateTime.UtcNow;
            var expireInSeconds = Convert.ToInt32(timeSpan.TotalSeconds);

            var result = new AuthenticateResultModel
            {
                AccessToken = OAuthBearerOptions.AccessTokenFormat.Protect(ticket),
                ExpireInSeconds = expireInSeconds
            };


            return new AjaxResponse(result);
        }


        [HttpGet]
        public AjaxResponse Logout()
        {
            //var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            //{
            //    IssuedUtc = context.Ticket.Properties.IssuedUtc,
            //    ExpiresUtc = DateTime.UtcNow.AddYears(1)
            //};
            //var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            ////_refreshTokens.TryAdd(guid, context.Ticket);
            //_refreshTokens.TryAdd(guid, refreshTokenTicket);

            //// consider storing only the hash of the handle
            //context.SetToken(guid);
            _authenticationManager.SignOut();
            return new AjaxResponse();
        }


        /// <summary>
        ///     获取登陆信息返回的结果
        /// </summary>
        /// <param name="usernameOrEmailAddress"></param>
        /// <param name="password"></param>
        /// <param name="tenancyName"></param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress,
            string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result,
                        usernameOrEmailAddress, tenancyName);
            }
        }
    }
}