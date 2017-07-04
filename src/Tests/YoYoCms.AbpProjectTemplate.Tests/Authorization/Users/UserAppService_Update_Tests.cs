using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Microsoft.AspNet.Identity;
using YoYoCms.AbpProjectTemplate.Authorization.Roles;
using Shouldly;
using Xunit;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;
using YoYoCms.AbpProjectTemplate.UserManagement.Users.Dto;

namespace YoYoCms.AbpProjectTemplate.Tests.Authorization.Users
{
    public class UserAppService_Update_Tests : UserAppServiceTestBase
    {
        [Fact]
        public async Task Update_User_Basic_Tests()
        {
            //Arrange
            var managerRole = CreateRole("Manager");
            var adminUser = await GetUserByUserNameOrNullAsync(User.AdminUserName);

            //Act
            await UserAppService.CreateOrUpdateUser(
                new CreateOrUpdateUserInput
                {
                    User = new UserEditDto
                    {
                        Id = adminUser.Id,
                        EmailAddress = "admin1@abp.com",
                        Name = "System1",
                        Surname = "Admin2",
                        Password = "123qwe",
                        UserName = adminUser.UserName
                    },
                    AssignedRoleNames = new[] { "Manager" }
                });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                //Get created user
                var updatedAdminUser = await GetUserByUserNameOrNullAsync(adminUser.UserName, includeRoles: true);
                updatedAdminUser.ShouldNotBe(null);
                updatedAdminUser.Id.ShouldBe(adminUser.Id);

                //Check some properties
                updatedAdminUser.EmailAddress.ShouldBe("admin1@abp.com");
                updatedAdminUser.TenantId.ShouldBe(AbpSession.TenantId);

                new PasswordHasher()
                    .VerifyHashedPassword(updatedAdminUser.Password, "123qwe")
                    .ShouldBe(PasswordVerificationResult.Success);

                //Check roles
                updatedAdminUser.Roles.Count.ShouldBe(1);
                updatedAdminUser.Roles.Any(ur => ur.RoleId == managerRole.Id).ShouldBe(true);
            });
        }

        [Fact]
        public async Task Should_Not_Update_User_With_Duplicate_Username_Or_EmailAddress()
        {
            //Arrange

            CreateTestUsers();
            var jnashUser = await GetUserByUserNameOrNullAsync("jnash");

            //Act

            //Try to update with existing name
            var exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await UserAppService.CreateOrUpdateUser(
                    new CreateOrUpdateUserInput
                    {
                        User = new UserEditDto
                               {
                                   Id = jnashUser.Id,
                                   EmailAddress = "jnsh2000@testdomain.com",
                                   Name = "John",
                                   Surname = "Nash",
                                   UserName = "adams_d", //Changed user name to an existing user
                                   Password = "123qwe"
                               },
                        AssignedRoleNames = new string[0]
                    }));

            exception.Message.ShouldContain("adams_d");

            //Try to update with existing email address
            exception = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await UserAppService.CreateOrUpdateUser(
                    new CreateOrUpdateUserInput
                    {
                        User = new UserEditDto
                               {
                                   Id = jnashUser.Id,
                                   EmailAddress = "adams_d@gmail.com", //Changed email to an existing user
                                   Name = "John",
                                   Surname = "Nash",
                                   UserName = "jnash",
                                   Password = "123qwe"
                               },
                        AssignedRoleNames = new string[0]
                    }));

            exception.Message.ShouldContain("adams_d@gmail.com");
        }

        [MultiTenantFact]
        public async Task Should_Remove_From_Role()
        {
            LoginAsHostAdmin();

            //Arrange
            var adminUser = await GetUserByUserNameOrNullAsync(User.AdminUserName);
            await UsingDbContextAsync(async context =>
            {
                var roleCount = await context.UserRoles.CountAsync(ur => ur.UserId == adminUser.Id);
                roleCount.ShouldBeGreaterThan(0); //There should be 1 role at least
            });

            //Act
            await UserAppService.CreateOrUpdateUser(
                new CreateOrUpdateUserInput
                {
                    User = new UserEditDto //Not changing user properties
                    {
                        Id = adminUser.Id,
                        EmailAddress = adminUser.EmailAddress,
                        Name = adminUser.Name,
                        Surname = adminUser.Surname,
                        UserName = adminUser.UserName,
                        Password = null
                    },
                    AssignedRoleNames = new string[0] //Just deleting all roles
                });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                var roleCount = await context.UserRoles.CountAsync(ur => ur.UserId == adminUser.Id);
                roleCount.ShouldBe(0);
            });
        }

        protected Role CreateRole(string roleName)
        {
            return UsingDbContext(context => context.Roles.Add(new Role(AbpSession.TenantId, roleName, roleName)));
        }
    }
}
