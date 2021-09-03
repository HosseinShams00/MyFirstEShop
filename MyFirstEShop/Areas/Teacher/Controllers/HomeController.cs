using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class HomeController : Controller
    {
        public int UserId
        {
            get
            {
                return int.Parse(User.FindFirst("UserId").Value);
            }
        }

        private readonly ITeacherRepository teacherRepository;
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public HomeController(ITeacherRepository _teacherRepository, IProductRepository _productRepository, ICategoryRepository _categoryRepository)
        {
            teacherRepository = _teacherRepository;
            productRepository = _productRepository;
            categoryRepository = _categoryRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }
        public IActionResult Dashboard()
        {
            ViewBag.SuccessAlert = TempData["Alert"];

            return View(productRepository.GetProductDetailListForTeacher(UserId));
        }


        public IActionResult AddNewProduct()
        {
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
            var isChanged = productRepository.ChangeProductStatus(ProductStatus.Suspension, productId);

            if (isChanged == false)
            {
                TempData["Alert"] = "Faild";
                return RedirectToAction("Dashboard");

            }

            TempData["Alert"] = "Suspension";
            return RedirectToAction("Dashboard");
        }


        public IActionResult EditProductDetail(int productId)
        {
            var productView = productRepository.GetProductViewModel(UserId, productId);
            return View(productView);
        }

        [HttpPost]
        public IActionResult EditProductDetail(ProductViewModel productViewModel)
        {
            productRepository.EditProductDetail(productViewModel);

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
