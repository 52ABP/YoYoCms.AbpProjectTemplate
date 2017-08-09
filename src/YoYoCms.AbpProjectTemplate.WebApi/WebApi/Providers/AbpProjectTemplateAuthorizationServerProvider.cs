using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using YoYoCms.AbpProjectTemplate.Authorization;
using YoYoCms.AbpProjectTemplate.Authorization.Roles;
using YoYoCms.AbpProjectTemplate.MultiTenancy;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.WebApi.Providers
{
    public class AbpProjectTemplateAuthorizationServerProvider : OAuthAuthorizationServerProvider, ITransientDependency
    {
        

        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;

         private readonly LogInManager _logInManager;

        public AbpProjectTemplateAuthorizationServerProvider(AbpLoginResultTypeHelper abpLoginResultTypeHelper, LogInManager logInManager)
        {
        
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            var isValidClient = string.CompareOrdinal(clientId, "app") == 0 &&
                                string.CompareOrdinal(clientSecret, "app") == 0;
            if (isValidClient)
            {
                context.OwinContext.Set("as:client_id", clientId);
                context.Validated(clientId);
            }
            else
            {
                context.SetError("invalid client");
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var tenancyName = context.Request.Query["tenancyName"];
            var result = await GetLoginResultAsync(context.UserName, context.Password, tenancyName);
            if (result.Result == AbpLoginResultType.Success)
            {
                 var claimsIdentity = new ClaimsIdentity(result.Identity);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                var ticket = new AuthenticationTicket(claimsIdentity, new AuthenticationProperties());
                context.Validated(ticket);
            }
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.OwinContext.Get<string>("as:client_id");
            var currentClient = context.ClientId;

            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.Rejected();
                return;
            }

            // chance to change authentication ticket for refresh token requests
            var newId = new ClaimsIdentity(context.Ticket.Identity);
            newId.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTicket);
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