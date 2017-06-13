using System;
using System.Linq;
using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.RealTime;
using Abp.UI;
using YoYoCms.AbpProjectTemplate.Authorization.Users;
using YoYoCms.AbpProjectTemplate.Friendships;
using YoYoCms.AbpProjectTemplate.Friendships.Cache;

namespace YoYoCms.AbpProjectTemplate.Chat
{
    [AbpAuthorize]
    public class ChatMessageManager : AbpProjectTemplateDomainServiceBase, IChatMessageManager
    {
        private readonly IFriendshipManager _friendshipManager;
        private readonly IChatCommunicator _chatCommunicator;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly UserManager _userManager;
        private readonly ITenantCache _tenantCache;
        private readonly IUserFriendsCache _userFriendsCache;
        private readonly IUserEmailer _userEmailer;
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        private readonly IChatFeatureChecker _chatFeatureChecker;

        public ChatMessageManager(
            IFriendshipManager friendshipManager,
            IChatCommunicator chatCommunicator,
            IOnlineClientManager onlineClientManager,
            UserManager userManager,
            ITenantCache tenantCache,
            IUserFriendsCache userFriendsCache,
            IUserEmailer userEmailer,
            IRepository<ChatMessage, long> chatMessageRepository,
            IChatFeatureChecker chatFeatureChecker)
        {
            _friendshipManager = friendshipManager;
            _chatCommunicator = chatCommunicator;
            _onlineClientManager = onlineClientManager;
            _userManager = userManager;
            _tenantCache = tenantCache;
            _userFriendsCache = userFriendsCache;
            _userEmailer = userEmailer;
            _chatMessageRepository = chatMessageRepository;
            _chatFeatureChecker = chatFeatureChecker;
        }

        public void SendMessage(UserIdentifier sender, UserIdentifier receiver, string message, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId)
        {
            CheckReceiverExists(receiver);

            _chatFeatureChecker.CheckChatFeatures(sender.TenantId, receiver.TenantId);
            
            var friendshipState = _friendshipManager.GetFriendshipOrNull(sender, receiver)?.State;
            if (friendshipState == FriendshipState.Blocked)
            {
                throw new UserFriendlyException(L("UserIsBlocked"));
            }
            
            HandleSenderToReceiver(sender, receiver, message);
            HandleReceiverToSender(sender, receiver, message);
            HandleSenderUserInfoChange(sender, receiver, senderTenancyName, senderUserName, senderProfilePictureId);
        }

        private void CheckReceiverExists(UserIdentifier receiver)
        {
            var receiverUser = _userManager.GetUserOrNull(receiver);
            if (receiverUser == null)
            {
                throw new UserFriendlyException(L("TargetUserNotFoundProbablyDeleted"));
            }
        }

        [UnitOfWork]
        public virtual long Save(ChatMessage message)
        {
            using (CurrentUnitOfWork.SetTenantId(message.TenantId))
            {
                return _chatMessageRepository.InsertAndGetId(message);
            }
        }

        [UnitOfWork]
        public virtual int GetUnreadMessageCount(UserIdentifier sender, UserIdentifier receiver)
        {
            using (CurrentUnitOfWork.SetTenantId(receiver.TenantId))
            {
                return _chatMessageRepository.Count(cm => cm.UserId == receiver.UserId && cm.TargetUserId == sender.UserId && cm.TargetTenantId == sender.TenantId && cm.ReadState == ChatMessageReadState.Unread);
            }
        }

        private void HandleSenderToReceiver(UserIdentifier senderIdentifier, UserIdentifier receiverIdentifier, string message)
        {
            var friendshipState = _friendshipManager.GetFriendshipOrNull(senderIdentifier, receiverIdentifier)?.State;
            if (friendshipState == null)
            {
                friendshipState = FriendshipState.Accepted;

                var receiverTenancyName = receiverIdentifier.TenantId.HasValue
                    ? _tenantCache.Get(receiverIdentifier.TenantId.Value).TenancyName
                    : null;

                var receiverUser = _userManager.GetUser(receiverIdentifier);
                _friendshipManager.CreateFriendship(
                    new Friendship(
                        senderIdentifier,
                        receiverIdentifier,
                        receiverTenancyName,
                        receiverUser.UserName,
                        receiverUser.ProfilePictureId,
                        friendshipState.Value)
                );
            }

            if (friendshipState.Value == FriendshipState.Blocked)
            {
                //Do not send message if receiver banned the sender
                return;
            }

            var sentMessage = new ChatMessage(
                senderIdentifier,
                receiverIdentifier,
                ChatSide.Sender,
                message,
                ChatMessageReadState.Read
            );

            Save(sentMessage);

            _chatCommunicator.SendMessageToClient(
                _onlineClientManager.GetAllByUserId(senderIdentifier),
                sentMessage
                );
        }

        private void HandleReceiverToSender(UserIdentifier senderIdentifier, UserIdentifier receiverIdentifier, string message)
        {
            var friendshipState = _friendshipManager.GetFriendshipOrNull(receiverIdentifier, senderIdentifier)?.State;

            if (friendshipState == null)
            {
                var senderTenancyName = senderIdentifier.TenantId.HasValue ?
                    _tenantCache.Get(senderIdentifier.TenantId.Value).TenancyName :
                    null;

                var senderUser = _userManager.GetUser(senderIdentifier);
                _friendshipManager.CreateFriendship(
                    new Friendship(
                        receiverIdentifier,
                        senderIdentifier,
                        senderTenancyName,
                        senderUser.UserName,
                        senderUser.ProfilePictureId,
                        FriendshipState.Accepted
                    )
                );
            }

            if (friendshipState == FriendshipState.Blocked)
            {
                //Do not send message if receiver banned the sender
                return;
            }

            var sentMessage = new ChatMessage(
                    receiverIdentifier,
                    senderIdentifier,
                    ChatSide.Receiver,
                    message,
                    ChatMessageReadState.Unread);

            Save(sentMessage);

            var clients = _onlineClientManager.GetAllByUserId(receiverIdentifier);
            if (clients.Any())
            {
                _chatCommunicator.SendMessageToClient(clients, sentMessage);
            }
            else if (GetUnreadMessageCount(senderIdentifier, receiverIdentifier) == 1)
            {
                var senderTenancyName = senderIdentifier.TenantId.HasValue ?
                    _tenantCache.Get(senderIdentifier.TenantId.Value).TenancyName :
                    null;

                _userEmailer.TryToSendChatMessageMail(
                    _userManager.GetUser(receiverIdentifier),
                    _userManager.GetUser(senderIdentifier).UserName,
                    senderTenancyName,
                    sentMessage
                );
            }
        }

        private void HandleSenderUserInfoChange(UserIdentifier sender, UserIdentifier receiver, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId)
        {
            var receiverCacheItem = _userFriendsCache.GetCacheItemOrNull(receiver);
            if (receiverCacheItem == null)
            {
                return;
            }

            var senderAsFriend = receiverCacheItem.Friends.FirstOrDefault(f => f.FriendTenantId == sender.TenantId && f.FriendUserId == sender.UserId);
            if (senderAsFriend == null)
            {
                return;
            }

            if (senderAsFriend.FriendTenancyName == senderTenancyName &&
                senderAsFriend.FriendUserName == senderUserName &&
                senderAsFriend.FriendProfilePictureId == senderProfilePictureId)
            {
                return;
            }

            var friendship = _friendshipManager.GetFriendshipOrNull(receiver, sender);
            if (friendship == null)
            {
                return;
            }

            friendship.FriendTenancyName = senderTenancyName;
            friendship.FriendUserName = senderUserName;
            friendship.FriendProfilePictureId = senderProfilePictureId;

            _friendshipManager.UpdateFriendship(friendship);
        }
    }
}