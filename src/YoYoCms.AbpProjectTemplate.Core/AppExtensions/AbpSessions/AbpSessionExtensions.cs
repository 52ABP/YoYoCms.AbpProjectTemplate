using System.Linq;
using System.Security.Claims;
using System.Threading;
using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;

namespace YoYoCms.AbpProjectTemplate.AppExtensions.AbpSessions
{
    public class AbpSessionExtensions : ClaimsAbpSession, IAbpSessionExtensions
    {
        public AbpSessionExtensions(IPrincipalAccessor principalAccessor, IMultiTenancyConfig multiTenancy, ITenantResolver tenantResolver, IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) : base(principalAccessor,multiTenancy,tenantResolver,sessionOverrideScopeProvider)
        {
        }

        public string UserName => GetKeyValue(AbpProjectTemplateConsts.ClaimTypes.UserName);
        public string EmailAddress => GetKeyValue(ClaimTypes.Email);
      
      

        private string GetKeyValue(string key)
        {
            var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            var claim = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == key);
            return string.IsNullOrEmpty(claim?.Value) ? null : claim.Value;
        }

    
    }
}