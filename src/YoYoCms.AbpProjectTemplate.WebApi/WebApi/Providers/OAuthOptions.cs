using System;
using Abp.Dependency;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace YoYoCms.AbpProjectTemplate.WebApi.Providers
{
    /// <summary>
    /// Class OAuthOptions.
    /// </summary>
    public class OAuthOptions
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
                var provider = IocManager.Instance.Resolve<SimpleAuthorizationServerProvider>();
                var refreshTokenProvider = IocManager.Instance.Resolve<SimpleRefreshTokenProvider>();
                _serverOptions = new OAuthAuthorizationServerOptions
                {
                    TokenEndpointPath = new PathString("/oauth/token"),
                    Provider = provider,
                    RefreshTokenProvider = refreshTokenProvider,
                    AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(30),
                    AllowInsecureHttp = true
                };
            }
            return _serverOptions;
        }
    }
}