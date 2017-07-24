using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users
{
    public interface IUserLinkManager
    {
        Task Link(User firstUser, User secondUser);

        Task<bool> AreUsersLinked(UserIdentifier firstUserIdentifier, UserIdentifier secondUserIdentifier);

        Task Unlink(UserIdentifier userIdentifier);

        Task<UserAccount> GetUserAccountAsync(UserIdentifier userIdentifier);
    }
}