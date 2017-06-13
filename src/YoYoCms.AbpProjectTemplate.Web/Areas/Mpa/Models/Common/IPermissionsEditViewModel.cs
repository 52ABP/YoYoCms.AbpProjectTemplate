using System.Collections.Generic;
using YoYoCms.AbpProjectTemplate.Authorization.Permissions.Dto;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}