using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Localization;
using Abp.Timing;
using Abp.Zero;
using YoYoCms.AbpProjectTemplate.Auditing;
using YoYoCms.AbpProjectTemplate.Auditing.Dto;
using Shouldly;
using Xunit;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.Tests.Auditing
{
    public class AuditLogAppService_Tests : AppTestBase
    {
        private readonly IAuditLogAppService _auditLogAppService;

        public AuditLogAppService_Tests()
        {
            _auditLogAppService = Resolve<IAuditLogAppService>();
        }

        [Theory]
        [InlineData("en")]
        [InlineData("en-US")]
        [InlineData("en-GB")]
        public void Test1(string cultureName)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);

            Resolve<ILanguageManager>().CurrentLanguage.Name.ShouldBe("en");

            Resolve<ILocalizationManager>()
                .GetString(AbpZeroConsts.LocalizationSourceName, "Identity.UserNotInRole")
                .ShouldBe("User is not in role.");
        }

        [Fact]
        public async Task Should_Get_Audit_Logs()
        {
            //Arrange
            UsingDbContext(
                context =>
                {
                    context.AuditLogs.Add(
                        new AuditLog
                        {
                            TenantId = AbpSession.TenantId,
                            UserId = AbpSession.UserId,
                            ServiceName = "ServiceName-Test-1",
                            MethodName = "MethodName-Test-1",
                            Parameters = "{}",
                            ExecutionTime = Clock.Now.AddMinutes(-1),
                            ExecutionDuration = 123
                        });

                    context.AuditLogs.Add(
                        new AuditLog
                        {
                            TenantId = AbpSession.TenantId,
                            ServiceName = "ServiceName-Test-2",
                            MethodName = "MethodName-Test-2",
                            Parameters = "{}",
                            ExecutionTime = Clock.Now,
                            ExecutionDuration = 456
                        });
                });

            //Act
            var output = await _auditLogAppService.GetAuditLogs(new GetAuditLogsInput
            {
                StartDate = Clock.Now.AddMinutes(-10),
                EndDate = Clock.Now.AddMinutes(10)
            });

            output.TotalCount.ShouldBe(2);
            
            output.Items[0].ServiceName.ShouldBe("ServiceName-Test-2");
            output.Items[0].UserName.ShouldBe(null);

            output.Items[1].ServiceName.ShouldBe("ServiceName-Test-1");
            
            output.Items[1].UserName.ShouldBe(User.AdminUserName, StringCompareShould.IgnoreCase);
        }
    }
}
