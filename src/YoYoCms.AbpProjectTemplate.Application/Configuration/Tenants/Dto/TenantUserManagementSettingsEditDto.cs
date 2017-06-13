namespace YoYoCms.AbpProjectTemplate.Configuration.Tenants.Dto
{
    public class TenantUserManagementSettingsEditDto
    {
        public bool AllowSelfRegistration { get; set; }
        
        public bool IsNewRegisteredUserActiveByDefault { get; set; }

        public bool IsEmailConfirmationRequiredForLogin { get; set; }
        
        public bool UseCaptchaOnRegistration { get; set; }
    }
}