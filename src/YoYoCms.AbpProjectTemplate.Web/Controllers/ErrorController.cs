using System.Web.Mvc;
using Abp.Auditing;

namespace YoYoCms.AbpProjectTemplate.Web.Controllers
{
    public class ErrorController : AbpProjectTemplateControllerBase
    {
        [DisableAuditing]
        public ActionResult E404()
        {
            return View();
        }
    }
}