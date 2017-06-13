using Abp.AutoMapper;
using YoYoCms.AbpProjectTemplate.Editions.Dto;
using YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Common;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionForEditOutput))]
    public class CreateOrEditEditionModalViewModel : GetEditionForEditOutput, IFeatureEditViewModel
    {
        public bool IsEditMode
        {
            get { return Edition.Id.HasValue; }
        }

        public CreateOrEditEditionModalViewModel(GetEditionForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}