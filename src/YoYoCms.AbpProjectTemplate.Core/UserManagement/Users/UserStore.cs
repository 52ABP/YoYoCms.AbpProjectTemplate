using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using YoYoCms.AbpProjectTemplate.Authorization.Roles;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users
{
    /// <summary>
    /// Used to perform database operations for <see cref="UserManager"/>.
    /// </summary>
    public class UserStore : AbpUserStore<Role, User>
    {
        public UserStore(
            IRepository<User, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserClaim, long> userCliamRepository
            )
            : base(
                userRepository,
                userLoginRepository,
                userRoleRepository,
                roleRepository,
                userPermissionSettingRepository,
                unitOfWorkManager,
                userCliamRepository)
        {
        }
    }
}