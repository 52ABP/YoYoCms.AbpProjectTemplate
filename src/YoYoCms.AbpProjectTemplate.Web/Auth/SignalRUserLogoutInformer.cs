using System.Collections.Generic;
using Abp.Dependency;
using Abp.RealTime;
using Abp.Web.SignalR.Hubs;
using Castle.Core.Logging;
using Microsoft.AspNet.SignalR;
using YoYoCms.AbpProjectTemplate.Authorization.Users;

namespace YoYoCms.AbpProjectTemplate.Web.Auth
{
    public class SignalRUserLogoutInformer : IUserLogoutInformer, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private static IHubContext CommonHub
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<AbpCommonHub>();
            }
        }

        public void InformClients(IReadOnlyList<IOnlineClient> clients)
        {
            foreach (var client in clients)
            {
                var signalRClient = CommonHub.Clients.Client(client.ConnectionId);
                if (signalRClient == null)
                {
                    Logger.Debug("Can not get user " + client.UserId + " from SignalR hub!");
                    continue;
                }

                signalRClient.userLoggedOut();
            }
        }
    }
}