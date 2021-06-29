using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Repositories;
using MyFirstEShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace MyFirstEShop.Controllers
{
    [Authorize]
    public class TeacherPanelController : Controller
    {

        private readonly ITeacherRepository teacherRepository;
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public int UserId
        {
            get
            {
                return int.Parse(User.FindFirst("UserId").Value);
            }
        }
        public TeacherPanelController(ITeacherRepository _teacherRepository,IProductRepository _productRepository,ICategoryRepository _categoryRepository)
        {
            teacherRepository = _teacherRepository;
            productRepository = _productRepository;
            categoryRepository = _categoryRepository;
        }

        public IActionResult Index()
        {
            if (!teacherRepository.ExistTeacher(UserId))
            {
                return NotFound();
            }
            return RedirectToAction("Dashboard");
        }


        public IActionResult Dashboard()
        {
            ViewBag.SuccessAlert = TempData["Alert"];

            if (!teacherRepository.ExistTeacher(UserId))
            {
                return NotFound();
            }

            return View(productRepository.GetProductDetailListForTeacher(UserId));
        }


        public IActionResult AddNewProduct()
        {
            if (!teacherRepository.ExistTeacher(UserId))
            {
                return NotFound();
            }

            var Categories = categoryRepository.GetCategories();

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

                        var Categories = categoryRepository.GetCategories();
                        productModel.Categories = Categories;
                        return View(productModel);
                    }

                    if (productRepository.ExistProduct(productModel.Name))
                    {
                        ModelState.AddModelError("Name", "این محصول در سایت موجود میباشد لطفا یک نام دیگر انتخاب کنید");

                        var Categories = categoryRepository.GetCategories();
                        productModel.Categories = Categories;
                        return View(productModel);
                    }

                    string coverAddress = SaveProductCover(productModel.ProductImage);

                    productRepository.AddNewProduct(productModel, UserId, coverAddress);

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
            var isChanged = productRepository.ChangeProductStatus(ProductStatus.Suspension , productId);

            if ( isChanged == false )
            {
                TempData["Alert"] = "Faild";
                return RedirectToAction("Dashboard");

            }

            TempData["Alert"] = "Suspension";
            return RedirectToAction("Dashboard");
        }


        private string SaveProductCover(IFormFile formFile)
        {
            string safeFileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "ProductCovers",
                 safeFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);

            }

            return safeFileName;
        }

    }
}
