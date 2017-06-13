using System.Collections.Generic;
using Abp.Application.Services.Dto;
using YoYoCms.AbpProjectTemplate.Authorization.Permissions.Dto;

namespace YoYoCms.AbpProjectTemplate.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}