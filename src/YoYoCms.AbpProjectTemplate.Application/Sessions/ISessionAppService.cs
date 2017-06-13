using System.Threading.Tasks;
using Abp.Application.Services;
using YoYoCms.AbpProjectTemplate.Sessions.Dto;

namespace YoYoCms.AbpProjectTemplate.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
