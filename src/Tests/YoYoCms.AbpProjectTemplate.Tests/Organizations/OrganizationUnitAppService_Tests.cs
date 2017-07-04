using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Organizations;
using YoYoCms.AbpProjectTemplate.Organizations;
using YoYoCms.AbpProjectTemplate.Organizations.Dto;
using Shouldly;
using Xunit;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.Tests.Organizations
{
    public class OrganizationUnitAppService_Tests : AppTestBase
    {
        private readonly IOrganizationUnitAppService _organizationUnitAppService;

        public OrganizationUnitAppService_Tests()
        {
            _organizationUnitAppService = Resolve<IOrganizationUnitAppService>();
        }

        [Fact]
        public async Task Test_GetOrganizationUnits()
        {
            //Act
            var output = await _organizationUnitAppService.GetOrganizationUnits();

            //Assert
            output.Items.Count.ShouldBe(7);
        }


        [Fact]
        public async Task Test_GetOrganizationUnitUsers()
        {
            //Arrange
            var ou1 = GetOU("OU1");

            //Act
            await _organizationUnitAppService.GetOrganizationUnitUsers(
                new GetOrganizationUnitUsersInput
                {
                    Id = ou1.Id
                });

            //TODO: Assert
        }

        [Fact]
        public async Task Test_CreateOrganizationUnit()
        {
            //Arrange
            var ou11 = GetOU("OU11");
            var newChildName = Guid.NewGuid().ToString();

            //Act
            await _organizationUnitAppService.CreateOrganizationUnit(new CreateOrganizationUnitInput
            {
                ParentId = ou11.Id,
                DisplayName = newChildName
            });

            //Assert
            UsingDbContext(context =>
            {
                var newOu = context.OrganizationUnits.FirstOrDefault(ou => ou.DisplayName == newChildName);
                newOu.ShouldNotBeNull();
                newOu.ParentId.ShouldBe(ou11.Id);
            });
        }

        [Fact]
        public async Task Test_UpdateOrganizationUnit()
        {
            //Arrange
            var ou11 = GetOU("OU11");

            //Act
            await _organizationUnitAppService.UpdateOrganizationUnit(new UpdateOrganizationUnitInput
            {
                Id = ou11.Id,
                DisplayName = "new ou11 display name"
            });

            //Assert
            GetOU(ou11.Id).DisplayName.ShouldBe("new ou11 display name");
        }

        [Fact]
        public async Task Test_MoveOrganizationUnit()
        {
            //Arrange
            var ou11 = GetOU("OU11");
            var ou12 = GetOU("OU12");

            //Act
            var output = await _organizationUnitAppService.MoveOrganizationUnit(new MoveOrganizationUnitInput
            {
                Id = ou11.Id,
                NewParentId = ou12.Id
            });

            //Assert
            output.ParentId.ShouldBe(ou12.Id);
            output.Code.ShouldBe(OrganizationUnit.CreateCode(1, 2, 1));
        }

        [Fact]
        public async Task Test_DeleteOrganizationUnit()
        {
            //Arrange
            var ou11 = GetOU("OU11");

            UsingDbContext(context =>
            {
                context.Users.FirstOrDefault(u => u.Id == AbpSession.UserId.Value && u.TenantId == AbpSession.TenantId.Value).ShouldNotBeNull();
            });

            //Act
            await _organizationUnitAppService.DeleteOrganizationUnit(new EntityDto<long>(ou11.Id));

            //Assert
            GetOU(ou11.Id).IsDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task Test_AddUserToOrganizationUnit()
        {
            //Arrange
            var ou12 = GetOU("OU12");
            var admin = GetUserByUserName(User.AdminUserName);

            //Act
            await _organizationUnitAppService.AddUserToOrganizationUnit(
                new UserToOrganizationUnitInput
                {
                    UserId = admin.Id,
                    OrganizationUnitId = ou12.Id
                });

            //Assert
            
            //check from database
            UsingDbContext(context => context.UserOrganizationUnits.FirstOrDefault(uou => uou.OrganizationUnitId == ou12.Id && uou.UserId == admin.Id)).ShouldNotBeNull();
            
            //Check also from app service
            var output = await _organizationUnitAppService.GetOrganizationUnitUsers(new GetOrganizationUnitUsersInput { Id = ou12.Id });
            output.Items.FirstOrDefault(u => u.Id == admin.Id).ShouldNotBeNull();
        }

        [Fact]
        public async Task Test_RemoveFromOrganizationUnit()
        {
            //Arrange
            var ou12 = GetOU("OU12");
            var admin = GetUserByUserName(User.AdminUserName);

            UsingDbContext(context => context.UserOrganizationUnits.Add(new UserOrganizationUnit(AbpSession.TenantId, admin.Id, ou12.Id)));

            //Act
            await _organizationUnitAppService.RemoveUserFromOrganizationUnit(
                new UserToOrganizationUnitInput
                {
                    UserId = admin.Id,
                    OrganizationUnitId = ou12.Id
                });

            //check from database
            UsingDbContext(context => context.UserOrganizationUnits.FirstOrDefault(uou => uou.OrganizationUnitId == ou12.Id && uou.UserId == admin.Id)).ShouldBeNull();
        }

        private OrganizationUnit GetOU(string diplayName)
        {
            var organizationUnit = UsingDbContext(context => context.OrganizationUnits.FirstOrDefault(ou => ou.DisplayName == diplayName));
            organizationUnit.ShouldNotBeNull();

            return organizationUnit;
        }

        private OrganizationUnit GetOU(long id)
        {
            var organizationUnit = UsingDbContext(context => context.OrganizationUnits.FirstOrDefault(ou => ou.Id == id));
            organizationUnit.ShouldNotBeNull();

            return organizationUnit;
        }
    }
}
