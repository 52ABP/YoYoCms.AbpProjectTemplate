using Abp.AutoMapper;
using YoYoCms.AbpProjectTemplate.Authorization.Roles.Dto;
using YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Common;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode
        {
            get { return Role.Id.HasValue; }
        }

        public CreateOrEditRoleModalViewModel(GetRoleForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}