using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YoYoCms.AbpProjectTemplate.Authorization.Users;
using YoYoCms.AbpProjectTemplate.Authorization.Users.Dto;

namespace YoYoCms.AbpProjectTemplate.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserAppService _userAppService;

        public HomeController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }


        public async Task<ActionResult> Index()
        {

 var dto=      await     _userAppService.GetUsers(new GetUsersInput());

            ViewBag.Title = "Home Page";

            return View(dto);
        }
    }
}
