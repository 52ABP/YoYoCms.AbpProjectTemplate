using System.Collections.Generic;
using YoYoCms.AbpProjectTemplate.Caching.Dto;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}