using Abp.Application.Services;
using YoYoCms.AbpProjectTemplate.Tenants.Dashboard.Dto;

namespace YoYoCms.AbpProjectTemplate.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();
    }
}
