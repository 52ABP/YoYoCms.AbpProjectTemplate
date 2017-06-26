using System.Web.Helpers;
using System.Web.Mvc;

namespace YoYoCms.AbpProjectTemplate.Web.Controllers
{
    public class HomeController : AbpProjectTemplateControllerBase
    {
         public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 管理端Vue项目首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ForntProj ()
        { 
            return File(System.IO.File.Open(Server.MapPath("/Assets/dist/index.html"), System.IO.FileMode.Open), "text/html");
        }
	}
}