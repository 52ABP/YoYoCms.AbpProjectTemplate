using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users
{
    public class UserLinkManager : AbpProjectTemplateDomainServiceBase, IUserLinkManager
    {
        private readonly IRepository<UserAccount, long> _userAccountRepository;

        public UserLinkManager(IRepository<UserAccount, long> userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        [UnitOfWork]
        public virtual async Task Link(User firstUser, User secondUser)
        {
            var firstUserAccount = await GetUserAccountAsync(firstUser.ToUserIdentifier());
            var secondUserAccount = await GetUserAccountAsync(secondUser.ToUserIdentifier());

            var userLinkId = firstUserAccount.UserLinkId ?? firstUserAccount.Id;
            firstUserAccount.UserLinkId = userLinkId;

            var userAccountsToLink = secondUserAccount.UserLinkId.HasValue
                ? _userAccountRepository.GetAllList(ua => ua.UserLinkId == secondUserAccount.UserLinkId.Value)
                : new List<UserAccount> { secondUserAccount };

            userAccountsToLink.ForEach(u =>
            {
                u.UserLinkId = userLinkId;
            });

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [UnitOfWork]
        public virtual async Task<bool> AreUsersLinked(UserIdentifier firstUserIdentifier, UserIdentifier secondUserIdentifier)
        {
            var firstUserAccount = await GetUserAccountAsync(firstUserIdentifier);
            var secondUserAccount = await GetUserAccountAsync(secondUserIdentifier);

            if (!firstUserAccount.UserLinkId.HasValue || !secondUserAccount.UserLinkId.HasValue)
            {
                return false;
            }

            return firstUserAccount.UserLinkId == secondUserAccount.UserLinkId;
        }

        [UnitOfWork]
        public virtual async Task Unlink(UserIdentifier userIdentifier)
        {
            var targetUserAccount = await GetUserAccountAsync(userIdentifier);
            targetUserAccount.UserLinkId = null;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [UnitOfWork]
        public virtual async Task<UserAccount> GetUserAccountAsync(UserIdentifier userIdentifier)
        {
            return await _userAccountRepository.FirstOrDefaultAsync(ua => ua.TenantId == userIdentifier.TenantId && ua.UserId == userIdentifier.UserId);
        }
    }
}
