namespace YoYoCms.AbpProjectTemplate.Web.Models.Account
{
    public class LoginFormViewModel
    {
        public string TenancyName { get; set; }
        
        public string SuccessMessage { get; set; }
        
        public string UserNameOrEmailAddress { get; set; }

        public bool IsSelfRegistrationEnabled { get; set; }
    }
}