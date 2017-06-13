using Abp.Application.Features;
using Abp.AutoMapper;
using Abp.UI.Inputs;

namespace YoYoCms.AbpProjectTemplate.Editions.Dto
{
    [AutoMapFrom(typeof(Feature))]
    public class FlatFeatureDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public IInputType InputType { get; set; }
    }
}