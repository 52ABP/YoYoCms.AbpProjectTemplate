using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using YoYoCms.AbpProjectTemplate.UserManagement.Users.Dto;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users
{
    [AbpAuthorize]
    public class UserLoginAppService : AbpProjectTemplateAppServiceBase, IUserLoginAppService
    {
        private readonly IRepository<UserLoginAttempt, long> _userLoginAttemptRepository;

        public UserLoginAppService(IRepository<UserLoginAttempt, long> userLoginAttemptRepository)
        {
            _userLoginAttemptRepository = userLoginAttemptRepository;
        }

        [DisableAuditing]
        public async Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts()
        {
            var userId = AbpSession.GetUserId();

            var loginAttempts = await _userLoginAttemptRepository.GetAll()
                .Where(la => la.UserId == userId)
                .OrderByDescending(la => la.CreationTime)
                .Take(10)
                .ToListAsync();

            return new ListResultDto<UserLoginAttemptDto>(loginAttempts.MapTo<List<UserLoginAttemptDto>>());
        }
    }
}