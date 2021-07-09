using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstEShop.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {

        
        public IActionResult Index()
        {
            return View();
        }
    }
}
