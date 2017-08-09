using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.Tests.Authorization.Users
{
    public abstract class UserAppServiceTestBase : AppTestBase
    {
        protected readonly IUserAppService UserAppService;

        protected UserAppServiceTestBase()
        {
            UserAppService = Resolve<IUserAppService>();
        }

        protected void CreateTestUsers()
        {
            //Note: There is a default "admin" user also

            UsingDbContext(
                context =>
                {
                    context.Users.Add(CreateUserEntity("jnash", "John", "Nash", "jnsh2000@testdomain.com"));
                    context.Users.Add(CreateUserEntity("adams_d", "Douglas", "Adams", "adams_d@gmail.com"));
                    context.Users.Add(CreateUserEntity("artdent", "Arthur", "Dent", "ArthurDent@yahoo.com"));
                });
        }

        protected User CreateUserEntity(string userName, string name, string surname, string emailAddress)
        {
            return new User
                   {
                       EmailAddress = emailAddress,
                       IsEmailConfirmed = true,
                       Name = name,
                       Surname = surname,
                       UserName = userName,
                       Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==", //123qwe
                       TenantId = AbpSession.TenantId
                   };
        }
    }
}