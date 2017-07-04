using System.Collections.Generic;
using YoYoCms.AbpProjectTemplate.Authorization.Permissions.Dto;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}