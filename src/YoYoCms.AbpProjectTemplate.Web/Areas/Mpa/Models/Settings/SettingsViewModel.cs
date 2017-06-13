using System.Collections.Generic;
using Abp.Application.Services.Dto;
using YoYoCms.AbpProjectTemplate.Configuration.Tenants.Dto;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}