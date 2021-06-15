using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Data;
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

        public IActionResult Detail(int ProductId)
        {
            var product = DbContext.Products.Include(include => include.ProductOtherInfo).Include(w => w.Teacher).FirstOrDefault(q => q.Id == ProductId);
            if (product != null)
                return View(ProductId);
            else
                return NotFound();

        }

    }
}
