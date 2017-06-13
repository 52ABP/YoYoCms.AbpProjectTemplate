using System;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;

namespace YoYoCms.AbpProjectTemplate.Friendships
{
    public class FriendshipManager : AbpProjectTemplateDomainServiceBase, IFriendshipManager
    {
        private readonly IRepository<Friendship, long> _friendshipRepository;

        public FriendshipManager(IRepository<Friendship, long> friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
        }

        [UnitOfWork]
        public void CreateFriendship(Friendship friendship)
        {
            if (friendship.TenantId == friendship.FriendTenantId &&
                friendship.UserId == friendship.FriendUserId)
            {
                throw new UserFriendlyException(L("YouCannotBeFriendWithYourself"));
            }

            using (CurrentUnitOfWork.SetTenantId(friendship.TenantId))
            {
                _friendshipRepository.Insert(friendship);
                CurrentUnitOfWork.SaveChanges();
            }
        }

        [UnitOfWork]
        public void UpdateFriendship(Friendship friendship)
        {
            using (CurrentUnitOfWork.SetTenantId(friendship.TenantId))
            {
                _friendshipRepository.Update(friendship);
                CurrentUnitOfWork.SaveChanges();
            }
        }

        [UnitOfWork]
        public Friendship GetFriendshipOrNull(UserIdentifier user, UserIdentifier probableFriend)
        {
            using (CurrentUnitOfWork.SetTenantId(user.TenantId))
            {
                return _friendshipRepository.FirstOrDefault(friendship =>
                                    friendship.UserId == user.UserId &&
                                    friendship.TenantId == user.TenantId &&
                                    friendship.FriendUserId == probableFriend.UserId &&
                                    friendship.FriendTenantId == probableFriend.TenantId);
            }
        }

        [UnitOfWork]
        public void BanFriend(UserIdentifier userIdentifier, UserIdentifier probableFriend)
        {
            var friendship = GetFriendshipOrNull(userIdentifier, probableFriend);
            if (friendship == null)
            {
                throw new ApplicationException("Friendship does not exist between " + userIdentifier + " and " + probableFriend);
            }

            friendship.State = FriendshipState.Blocked;
            UpdateFriendship(friendship);
        }

        [UnitOfWork]
        public void AcceptFriendshipRequest(UserIdentifier userIdentifier, UserIdentifier probableFriend)
        {
            var friendship = GetFriendshipOrNull(userIdentifier, probableFriend);
            if (friendship == null)
            {
                throw new ApplicationException("Friendship does not exist between " + userIdentifier + " and " + probableFriend);
            }

            friendship.State = FriendshipState.Accepted;
            UpdateFriendship(friendship);
        }
    }
}