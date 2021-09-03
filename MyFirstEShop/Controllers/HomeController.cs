using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Models;
using System.Diagnostics;
using MyFirstEShop.Attributes;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Controllers
{
    [TypeFilter(typeof(CheckUserSecurityStampAttribute))]
    public class HomeController : Controller
    {
        private readonly IProductRepository ProductRepository;

        public HomeController(IProductRepository productRepository)
        {
            ProductRepository = productRepository;
        }

        public IActionResult Index()
        {
            var products = ProductRepository.GetProducts();
            return View(products);
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/Error/{ErrorCode}")]
        public IActionResult Error(int ErrorCode)
        {
            return NotFound();
        }
    }

}
