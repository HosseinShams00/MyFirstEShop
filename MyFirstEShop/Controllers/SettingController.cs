using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Attributes;

namespace MyFirstEShop.Controllers
{
    [Authorize]
    [TypeFilter(typeof(CheckUserSecurityStampAttribute))]
    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
