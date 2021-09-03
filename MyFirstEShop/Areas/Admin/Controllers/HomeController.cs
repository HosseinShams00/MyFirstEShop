using Microsoft.AspNetCore.Mvc;

namespace MyFirstEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }
        public IActionResult Dashboard()
        {
            return View();
        }

    }
}
