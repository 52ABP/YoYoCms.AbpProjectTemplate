using System.Web.Mvc;
using Abp.Auditing;
using Abp.Web.Mvc.Authorization;

namespace YoYoCms.AbpProjectTemplate.Web.Controllers
{
   
    public class ApplicationController : AbpProjectTemplateControllerBase
    {
       

        /// <summary>
        /// 管理端Vue项目首页
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public ActionResult Vue()
        {
           
            return File(System.IO.File.Open(Server.MapPath("/Assets/dist/index.html"), System.IO.FileMode.Open), "text/html");
        }


    }
}