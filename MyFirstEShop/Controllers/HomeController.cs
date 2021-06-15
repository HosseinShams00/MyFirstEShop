using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyFirstEShop.Data;
using Microsoft.EntityFrameworkCore;

namespace MyFirstEShop.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext DbContext { get; set; }

        public HomeController(MyDbContext _DbContext)
        {
            DbContext = _DbContext;
        }

        public IActionResult Index()
        {
            var pro = DbContext.Products.ToList();

            return View(pro);

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
