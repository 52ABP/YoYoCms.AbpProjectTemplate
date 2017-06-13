using System.Web.Mvc;
using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using YoYoCms.AbpProjectTemplate.Authorization;
using YoYoCms.AbpProjectTemplate.Web.Controllers;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Controllers
{
    [DisableAuditing]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogsController : AbpProjectTemplateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}