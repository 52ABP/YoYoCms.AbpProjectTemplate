using System;
using Abp.Dependency;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace YoYoCms.AbpProjectTemplate.WebAppApi.Api.Providers
{
    /// <summary>
    /// Class OAuthOptions.
    /// </summary>
    public class AbpProjectTemplateOAuthOptions
    {
        /// <summary>
        /// Gets or sets the server options.
        /// </summary>
        /// <value>The server options.</value>
        private static OAuthAuthorizationServerOptions _serverOptions;

        /// <summary>
        /// Creates the server options.
        /// </summary>
        /// <returns>OAuthAuthorizationServerOptions.</returns>
        public static OAuthAuthorizationServerOptions CreateServerOptions()
        {
            if (_serverOptions == null)
            {
                var provider = IocManager.Instance.Resolve<AbpProjectTemplateAuthorizationServerProvider>();
                var refreshTokenProvider = IocManager.Instance.Resolve<AbpProjectTemplateRefreshTokenProvider>();
                _serverOptions = new OAuthAuthorizationServerOptions
                {
                    TokenEndpointPath = new PathString("/oauth/token"),
                    Provider = provider,
                    RefreshTokenProvider = refreshTokenProvider,
                    AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(30),
                    AllowInsecureHttp = true
                };
            }
            //todo:考虑在最外面包一层数据，感觉问题不大。。下午研究研究，看看可以实验下不。
            return _serverOptions;
        }
    }
}