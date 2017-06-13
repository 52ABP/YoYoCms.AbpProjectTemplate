using Abp.Notifications;
using YoYoCms.AbpProjectTemplate.Dto;

namespace YoYoCms.AbpProjectTemplate.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}