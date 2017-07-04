using System.ComponentModel.DataAnnotations;

namespace YoYoCms.AbpProjectTemplate.UserManagement.Users.Dto
{
    public class LinkToUserInput 
    {
        public string TenancyName { get; set; }

        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}