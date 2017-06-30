using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using YoYoCms.AbpProjectTemplate.AppExtensions.Attribute;
using YoYoCms.AbpProjectTemplate.Caching.Dto;

namespace YoYoCms.AbpProjectTemplate.Caching
{
    public interface ICachingAppService : IApplicationService
    {
        [WebAppApi]
        ListResultDto<CacheDto> GetAllCaches();

        Task ClearCache(EntityDto<string> input);
        [WebAppApi]
        Task ClearAllCaches();
    }
}
