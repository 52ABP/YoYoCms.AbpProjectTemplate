using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Models.Common.Modals;
using YoYoCms.AbpProjectTemplate.Web.Controllers;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class CommonController : AbpProjectTemplateControllerBase
    {
        public PartialViewResult LookupModal(LookupModalViewModel model)
        {
            return PartialView("Modals/_LookupModal", model);
        }
    }
}