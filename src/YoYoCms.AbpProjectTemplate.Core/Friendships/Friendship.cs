using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace YoYoCms.AbpProjectTemplate.Friendships
{
     public class Friendship : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        public long UserId { get; set; }

        public int? TenantId { get; set; }

        public long FriendUserId { get; set; }

        public int? FriendTenantId { get; set; }

        [Required]
        [MaxLength(AbpUserBase.MaxUserNameLength)]
        public string FriendUserName { get; set; }

        public string FriendTenancyName { get; set; }

        public Guid? FriendProfilePictureId { get; set; }

        public FriendshipState State { get; set; }

        public DateTime CreationTime { get; set; }

        public Friendship(UserIdentifier user, UserIdentifier probableFriend, string probableFriendTenancyName, string probableFriendUserName, Guid? probableFriendProfilePictureId, FriendshipState state)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (probableFriend == null)
            {
                throw new ArgumentNullException(nameof(probableFriend));
            }

            if (!Enum.IsDefined(typeof(FriendshipState), state))
            {
                throw new InvalidEnumArgumentException(nameof(state), (int)state, typeof(FriendshipState));
            }

            UserId = user.UserId;
            TenantId = user.TenantId;
            FriendUserId = probableFriend.UserId;
            FriendTenantId = probableFriend.TenantId;
            FriendTenancyName = probableFriendTenancyName;
            FriendUserName = probableFriendUserName;
            State = state;
            FriendProfilePictureId = probableFriendProfilePictureId;

            CreationTime = Clock.Now;
        }

        protected Friendship()
        {

        }
    }
}
