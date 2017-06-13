using Abp;

namespace YoYoCms.AbpProjectTemplate.Friendships.Cache
{
    public interface IUserFriendsCache
    {
        UserWithFriendsCacheItem GetCacheItem(UserIdentifier userIdentifier);

        UserWithFriendsCacheItem GetCacheItemOrNull(UserIdentifier userIdentifier);

        void ResetUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier);

        void IncreaseUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier, int change);

        void AddFriend(UserIdentifier userIdentifier, FriendCacheItem friend);

        void RemoveFriend(UserIdentifier userIdentifier, FriendCacheItem friend);

        void UpdateFriend(UserIdentifier userIdentifier, FriendCacheItem friend);
    }
}
