using System;
using Abp.AutoMapper;

namespace YoYoCms.AbpProjectTemplate.Friendships.Cache
{
    [AutoMapFrom(typeof(Friendship))]
    public class FriendCacheItem
    {
        public const string CacheName = "AppUserFriendCache";

        public long FriendUserId { get; set; }

        public int? FriendTenantId { get; set; }

        public string FriendUserName { get; set; }

        public string FriendTenancyName { get; set; }

        public Guid? FriendProfilePictureId { get; set; }

        public int UnreadMessageCount { get; set; }

        public FriendshipState State { get; set; }
    }
}