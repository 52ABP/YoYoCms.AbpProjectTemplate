using Abp.Auditing;
using System.Web.Mvc;

namespace YoYoCms.AbpProjectTemplate.Web.Controllers
{
    public class HomeController : AbpProjectTemplateControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

     
    }
}