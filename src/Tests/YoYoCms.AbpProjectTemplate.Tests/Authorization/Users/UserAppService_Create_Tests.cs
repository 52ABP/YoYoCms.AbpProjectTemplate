using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.UI;
using YoYoCms.AbpProjectTemplate.Authorization.Roles;
using YoYoCms.AbpProjectTemplate.MultiTenancy;
using Shouldly;
using Xunit;
using YoYoCms.AbpProjectTemplate.UserManagement.Users.Dto;

namespace YoYoCms.AbpProjectTemplate.Tests.Authorization.Users
{
    public class UserAppService_Create_Tests : UserAppServiceTestBase
    {
        [MultiTenantFact]
        public async Task Should_Create_User_For_Host()
        {
            LoginAsHostAdmin();

            await CreateUserAndTestAsync("jnash", "John", "Nash", "jnsh2000@testdomain.com", null);
            await CreateUserAndTestAsync("adams_d", "Douglas", "Adams", "adams_d@gmail.com", null, StaticRoleNames.Host.Admin);
        }

        [Fact]
        public async Task Should_Create_User_For_Tenant()
        {
            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;
            await CreateUserAndTestAsync("jnash", "John", "Nash", "jnsh2000@testdomain.com", defaultTenantId);
            await CreateUserAndTestAsync("adams_d", "Douglas", "Adams", "adams_d@gmail.com", defaultTenantId, StaticRoleNames.Tenants.Admin);
        }

        [Fact]
        public async Task Should_Not_Create_User_With_Duplicate_Username_Or_EmailAddress()
        {
            //Arrange
            CreateTestUsers();

            //Act
            await Assert.ThrowsAsync<UserFriendlyException>(
                async () =>
                    await UserAppService.CreateOrUpdateUser(
                        new CreateOrUpdateUserInput
                        {
                            User = new UserEditDto
                                   {
                                       EmailAddress = "john@nash.com",
                                       Name = "John",
                                       Surname = "Nash",
                                       UserName = "jnash", //Same username is added before (in CreateTestUsers)
                                       Password = "123qwe"
                                   },
                            AssignedRoleNames = new string[0]
                        }));
        }

        private async Task CreateUserAndTestAsync(string userName, string name, string surname, string emailAddress, int? tenantId, params string[] roleNames)
        {
            //Arrange
            AbpSession.TenantId = tenantId;

            //Act
            await UserAppService.CreateOrUpdateUser(
                new CreateOrUpdateUserInput
                {
                    User = new UserEditDto
                    {
                        EmailAddress = emailAddress,
                        Name = name,
                        Surname = surname,
                        UserName = userName,
                        Password = "123qwe"
                    },
                    AssignedRoleNames = roleNames
                });

            //Assert
            await UsingDbContext(async context =>
            {
                //Get created user
                var createdUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                createdUser.ShouldNotBe(null);

                //Check some properties
                createdUser.EmailAddress.ShouldBe(emailAddress);
                createdUser.TenantId.ShouldBe(tenantId);

                //Check roles
                if (roleNames.IsNullOrEmpty())
                {
                    createdUser.Roles.Count.ShouldBe(0);
                }
                else
                {
                    createdUser.Roles.Count.ShouldBe(roleNames.Length);
                    foreach (var roleName in roleNames)
                    {
                        var roleId = (await GetRoleAsync(roleName)).Id;
                        createdUser.Roles.Any(ur => ur.RoleId == roleId && ur.TenantId == tenantId).ShouldBe(true);
                    }
                }
            });
        }
    }
}
