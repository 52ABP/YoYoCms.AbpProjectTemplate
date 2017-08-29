using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.AspNet.Identity;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : AbpUser<User>
    {
        public const int MinPlainPasswordLength = 6;

        public const int MaxPhoneNumberLength = 24;

	    public const string DefaultPassword = "bb123456";


        public virtual Guid? ProfilePictureId { get; set; }

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        //Can add application specific user properties here
        [Required(AllowEmptyStrings = true)]
        public override string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public override string Surname { get; set; }

        [Required(AllowEmptyStrings = true)]
        public override string EmailAddress { get; set; }

        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }
        
        /// <summary>
        /// Creates admin <see cref="User"/> for a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <param name="password">Password</param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
                   {
                       TenantId = tenantId,
                       UserName = AdminUserName,
                       Name = AdminUserName,
                       Surname = AdminUserName,
                       EmailAddress = emailAddress,
                       Password = new PasswordHasher().HashPassword(password)
                   };
        }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }
    }
}