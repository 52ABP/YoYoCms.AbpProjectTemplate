using System;
using Abp;
using Abp.Dependency;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using Microsoft.AspNet.SignalR;
using YoYoCms.AbpProjectTemplate.Chat;

namespace YoYoCms.AbpProjectTemplate.Web.Chat.SignalR
{
    public class ChatHub : Hub, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to the session.
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        private readonly IChatMessageManager _chatMessageManager;
        private readonly ILocalizationManager _localizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatHub"/> class.
        /// </summary>
        public ChatHub(
            IChatMessageManager chatMessageManager, 
            ILocalizationManager localizationManager)
        {
            _chatMessageManager = chatMessageManager;
            _localizationManager = localizationManager;

            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        public string SendMessage(SendChatMessageInput input)
        {
            var sender = AbpSession.ToUserIdentifier();
            var receiver = new UserIdentifier(input.TenantId, input.UserId);

            try
            {
                _chatMessageManager.SendMessage(sender, receiver, input.Message, input.TenancyName, input.UserName, input.ProfilePictureId);
                return string.Empty;
            }
            catch (UserFriendlyException ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
                return ex.Message;
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
                return _localizationManager.GetSource("AbpWeb").GetString("InternalServerError");
            }
        }
    }
}
