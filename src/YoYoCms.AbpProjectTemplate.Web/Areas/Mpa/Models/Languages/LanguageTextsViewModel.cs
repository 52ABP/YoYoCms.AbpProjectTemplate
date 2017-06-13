using System.Collections.Generic;
using System.Web.Mvc;
using Abp.Localization;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Languages
{
    public class LanguageTextsViewModel
    {
        public string LanguageName { get; set; }

        public List<SelectListItem> Sources { get; set; }
        
        public List<LanguageInfo> Languages { get; set; }

        public string BaseLanguageName { get; set; }

        public string TargetValueFilter { get; set; }
        
        public string FilterText { get; set; }
    }
}