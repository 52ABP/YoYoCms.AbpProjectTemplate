using System.Collections.Generic;
using Abp.Application.Services.Dto;
using YoYoCms.AbpProjectTemplate.Configuration.Host.Dto;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<ComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}