using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace YoYoCms.AbpProjectTemplate.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}