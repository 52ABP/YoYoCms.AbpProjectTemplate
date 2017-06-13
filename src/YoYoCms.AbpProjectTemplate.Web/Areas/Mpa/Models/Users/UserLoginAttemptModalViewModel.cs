using System.Collections.Generic;
using YoYoCms.AbpProjectTemplate.Authorization.Users.Dto;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}