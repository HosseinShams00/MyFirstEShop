using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyFirstEShop.Data;
using MyFirstEShop.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MyFirstEShop.Controllers
{
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
    }

}
