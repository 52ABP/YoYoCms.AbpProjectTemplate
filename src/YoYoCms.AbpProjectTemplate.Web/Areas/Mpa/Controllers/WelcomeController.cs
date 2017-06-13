using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using YoYoCms.AbpProjectTemplate.Web.Controllers;

namespace YoYoCms.AbpProjectTemplate.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class WelcomeController : AbpProjectTemplateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}