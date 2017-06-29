using System.Threading.Tasks;
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

 
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
