using System.Threading.Tasks;
using Abp.Application.Services;
using YoYoCms.AbpProjectTemplate.UserManagement.Users.Profile.Dto;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users.Profile
{
    public interface IProfileAppService : IApplicationService
    {
        Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();

        Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);

        Task ChangePassword(ChangePasswordInput input);

        Task UpdateProfilePicture(UpdateProfilePictureInput input);

        // 上传头像
        Task UploadPortrait(UpdateProfilePictureInput imgData);

        Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting();
    }
}
