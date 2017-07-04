using System;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users.Dto
{
    public class GetUserForEditOutput
    {
        public Guid? ProfilePictureId { get; set; }

        public UserEditDto User { get; set; }

        public UserRoleDto[] Roles { get; set; }
    }
}