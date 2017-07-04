using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.MultiTenancy.Dto
{
    public class CreateTenantInput
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(Tenant.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(Tenant.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(User.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(User.MaxPasswordLength)]
        public string AdminPassword { get; set; }

        [MaxLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }
        
        public bool SendActivationEmail { get; set; }

        public int? EditionId { get; set; }

        public bool IsActive { get; set; }
    }
}