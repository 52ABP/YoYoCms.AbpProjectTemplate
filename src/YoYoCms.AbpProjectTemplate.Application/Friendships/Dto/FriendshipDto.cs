using System;
using Abp.AutoMapper;
using YoYoCms.AbpProjectTemplate.Friendships.Cache;

namespace YoYoCms.AbpProjectTemplate.Friendships.Dto
{
    [AutoMapFrom(typeof(FriendCacheItem), typeof(Friendship))]
    public class FriendDto
    {
        public long FriendUserId { get; set; }

        public int? FriendTenantId { get; set; }

        public string FriendUserName { get; set; }

        public string FriendTenancyName { get; set; }

        public Guid? FriendProfilePictureId { get; set; }

        public int UnreadMessageCount { get; set; }

        public bool IsOnline { get; set; }

        public FriendshipState State { get; set; }
    }
}
