using System.Threading.Tasks;
using Abp.Configuration;

namespace YoYoCms.AbpProjectTemplate.Timing
{
    public interface ITimeZoneService
    {
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);
    }
}
