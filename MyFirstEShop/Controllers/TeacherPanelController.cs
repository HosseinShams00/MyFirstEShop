using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Models;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyFirstEShop.Controllers
{
    [Authorize]
    public class TeacherPanelController : Controller
    {
        public MyDbContext DBContext { get; set; }
        public int UserId
        {
            get
            {
                return int.Parse(User.FindFirst("UserId").Value);
            }
        }
        public TeacherPanelController(MyDbContext dbContext)
        {
            DBContext = dbContext;
        }

        public IActionResult Index()
        {

            var user = DBContext.TeacherInfos
                .Include(i => i.Products)
                .FirstOrDefault(u => u.UserId == UserId);

            /// Check Teacher Is Exist
            if (user == null)
            {
                return NotFound();
            }



            return RedirectToAction("Dashboard");
        }


        public IActionResult Dashboard()
        {
            ViewBag.SuccessAlert = TempData["Alert"];

            var user = DBContext.TeacherInfos.Include(i => i.Info)
                .Include(i => i.Products)
                .ThenInclude(i => i.ProductOtherInfo)
                .FirstOrDefault(u => u.UserId == UserId);

            if (user == null)
            {
                return NotFound();
            }

            var products = user.Products
                .Select(i => new ProductDetailViewModel()
                {
                    ProductId = i.Id,
                    ProductName = i.Name,
                    Description = i.Description,
                    CoverAddress = i.ProductCoverName,
                    Price = i.Price,
                    Status = i.Status,
                    CourceLevel = i.ProductOtherInfo.CourceLevel,
                    NumberOfPurchases = i.ProductOtherInfo.NumberOfPurchases,
                    NumberOfVideos = i.ProductOtherInfo.NumberOfVideos,
                    StartDate = i.Create,
                    TeacherName = i.Teacher.Info.FirstName + " " + i.Teacher.Info.LastName
                })
                .ToList();

            return View(products);
        }


        public IActionResult AddNewProduct()
        {
            var user = DBContext.TeacherInfos.FirstOrDefault(u => u.UserId == UserId);
            if (user == null)
            {
                return NotFound();
            }

            var Categories = DBContext.Categories.ToList();

            return View(new ProductViewModel() { Categories = Categories });
        }


        [HttpPost]
        public IActionResult AddNewProduct(ProductViewModel productModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!productModel.ProductImage.ContentType.ToLower().Contains("image/"))
                    {
                        ModelState.AddModelError("ProductImage", "فرمت ورودی را لطفا عکس انتخاب کنید");

                        var Categories = DBContext.Categories.ToList();
                        productModel.Categories = Categories;
                        return View(productModel);
                    }

                    if (DBContext.Products.Any(i => i.Name.ToLower() == productModel.Name.Trim().ToLower()))
                    {
                        ModelState.AddModelError("Name", "این محصول در سایت موجود میباشد لطفا یک نام دیگر انتخاب کنید");

                        var Categories = DBContext.Categories.ToList();
                        productModel.Categories = Categories;
                        return View(productModel);
                    }

                    var teacher = DBContext.TeacherInfos.Single(i => i.UserId == UserId);

                    var category = DBContext.Categories.First(i => i.Id == productModel.CategoryId);

                    #region Save Image In Server

                    string safeFileName = Guid.NewGuid().ToString() + Path.GetExtension(productModel.ProductImage.FileName);

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "ProductCovers",
                         safeFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        productModel.ProductImage.CopyTo(stream);

                    }

                    #endregion

                    #region Save Product In DB

                    var product = new Product()
                    {
                        Categories = new List<Category>() { category },
                        Name = productModel.Name.Trim(),
                        Description = productModel.Description.Trim(),
                        ProductCoverName = safeFileName,
                        Price = productModel.Price,
                        HaveUpdate = true,
                        OffPercent = productModel.OffPercent,
                        TeacherId = teacher.Id,
                        Create = DateTime.Now,
                        EndSupport = null,

                    };

                    DBContext.Add(product);
                    DBContext.SaveChanges();

                    #endregion

                    #region Add Other Detail To Product

                    var productOtherInfo = new ProductOtherInfo()
                    {
                        ProdutId = product.Id,
                        CourceLevel = productModel.CourceLevel,
                        TotalTime = DateTime.Now

                    };

                    DBContext.Add(productOtherInfo);
                    DBContext.SaveChanges();

                    #endregion

                    /// Add Product To Teacher Table 
                    teacher.ProductId = product.Id;
                    DBContext.SaveChanges();

                    // Send Success Alert To Teacher
                    TempData["Alert"] = "Success";
                    return RedirectToAction("Dashboard");
                }
                catch (Exception ex)
                {
                    TempData["Alert"] = "Faild";
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View(productModel);
            }
        }
        

        [HttpPost]
        public IActionResult RemoveProduct(int productId)
        {
            var product = DBContext.Products.FirstOrDefault(p => p.Id == productId);

            if ( product == null )
            {
                TempData["Alert"] = "Faild";
                return RedirectToAction("Dashboard");

            }

            product.Status = ProductStatus.Suspension;

            DBContext.SaveChanges();

            TempData["Alert"] = "Suspension";
            return RedirectToAction("Dashboard");
        }


    }
}
