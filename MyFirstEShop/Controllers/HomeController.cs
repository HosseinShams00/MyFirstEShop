using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyFirstEShop.Data;
using MyFirstEShop.Models.ViewModels;
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
            var products = DbContext.Products
                .Include(i => i.Teacher)
                .ThenInclude(i => i.Info)
                .Where(i => i.Status == ProductStatus.Active)
                .Select(i => new HomeViewModel()
                {
                    ProductId = i.Id,
                    ProductName = i.Name,
                    CoverAddress = i.ProductCoverName,
                    Price = i.Price,
                    TeacherName = i.Teacher.Info.FirstName + " " + i.Teacher.Info.LastName
                })
                .ToList();

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
