using System.Threading.Tasks;
using Abp.Dependency;
using Castle.Core.Logging;
using Microsoft.AspNet.Identity;

namespace YoYoCms.AbpProjectTemplate.Identity
{
    public class IdentitySmsMessageService : IIdentityMessageService, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public IdentitySmsMessageService()
        {
            Logger = NullLogger.Instance;
        }

        public Task SendAsync(IdentityMessage message)
        {
            //TODO: Implement this service to send SMS to users. This is used by UserManager (ASP.NET Identity) on two factor auth.

            Logger.Warn("Sending SMS is not implemented! Message content:");
            Logger.Warn("Destination : " + message.Destination);
            Logger.Warn("Subject     : " + message.Subject);
            Logger.Warn("Body        : " + message.Body);

            return Task.FromResult(0);
        }
    }
}
