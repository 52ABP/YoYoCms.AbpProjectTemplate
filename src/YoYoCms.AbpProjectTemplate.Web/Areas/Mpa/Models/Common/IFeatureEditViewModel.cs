using System.Collections.Generic;
using Abp.Application.Services.Dto;
using YoYoCms.AbpProjectTemplate.Editions.Dto;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}