using System.Threading.Tasks;
using Abp.Application.Services;
using YoYoCms.AbpProjectTemplate.Configuration.Tenants.Dto;

namespace YoYoCms.AbpProjectTemplate.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);
    }
}
