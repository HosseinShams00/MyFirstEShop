using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Data;
using MyFirstEShop.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MyFirstEShop.Controllers
{
    public class ArticleController : Controller
    {
        public MyDbContext DbContext { get; set; }
        public ArticleController(MyDbContext myDbContext)
        {
            DbContext = myDbContext;
        }

        public IActionResult Detail(int productId, string productName)
        {
            var product = DbContext.Products
                .Include(include => include.ProductOtherInfo)
                .Include(w => w.Teacher)
                .ThenInclude(i => i.Info)
                .FirstOrDefault(q => q.Id == productId && q.Name == productName);

            if (product != null)
            {
                var productDetail = new ProductDetailViewModel()
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CoverAddress = product.ProductCoverName,
                    CourceLevel = product.ProductOtherInfo.CourceLevel,
                    StartDate = product.Create,
                    NumberOfPurchases = product.ProductOtherInfo.NumberOfPurchases,
                    NumberOfVideos = product.ProductOtherInfo.NumberOfVideos,
                    TeacherName = product.Teacher.Info.FirstName + " " + product.Teacher.Info.LastName

                };

                return View(productDetail);
            }
            else
                return NotFound();

        }

    }
}
