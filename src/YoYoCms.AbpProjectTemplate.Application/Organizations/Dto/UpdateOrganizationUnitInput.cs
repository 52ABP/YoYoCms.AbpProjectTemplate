using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace YoYoCms.AbpProjectTemplate.Organizations.Dto
{
    public class UpdateOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
    }
}