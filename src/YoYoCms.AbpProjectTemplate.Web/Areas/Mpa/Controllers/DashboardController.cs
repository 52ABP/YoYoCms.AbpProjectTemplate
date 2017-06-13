using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using YoYoCms.AbpProjectTemplate.Authorization;
using YoYoCms.AbpProjectTemplate.Web.Controllers;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class DashboardController : AbpProjectTemplateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}