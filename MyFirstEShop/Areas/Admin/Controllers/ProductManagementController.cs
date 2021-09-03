using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Areas.Admin.Models.ViewModel;
using MyFirstEShop.Attributes;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Repositories;
using System.Collections.Generic;


namespace MyFirstEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [TypeFilter(typeof(CheckUserSecurityStampAttribute))]
    public class ProductManagementController : Controller
    {
        private readonly IProductRepository productRepository;
        public ProductManagementController(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }

        #region Search 

        public IActionResult Search()
        {
            ViewBag.EditResult = TempData["EditResult"];
            return View(new List<Product>());
        }

        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditProductDetail })]
        public IActionResult Result(SearchProductViewModel searchProductViewModel)
        {
            var product = productRepository.SearchProduct(searchProductViewModel);
            return View("Search", product);
        }

        #endregion

        #region Edit Status

        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditProductStatus })]
        public IActionResult GetSuspendProduct()
        {
            var product = productRepository.GetSuspendProducts();
            ViewBag.ProductResult = TempData["ProductResult"];
            return View(product);
        }

        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditProductStatus })]
        public IActionResult RemoveProduct(int productId)
        {
            var result = productRepository.ChangeProductStatus(ProductStatus.Inactive, productId);

            if (result)
                TempData["ProductResult"] = "success";
            else
                TempData["ProductResult"] = "faild";

            return RedirectToAction("GetSuspendProduct");
        }

        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditProductStatus })]
        public IActionResult ActiveProduct(int productId)
        {
            var result = productRepository.ChangeProductStatus(ProductStatus.Active, productId);

            if (result)
                TempData["ProductResult"] = "success";
            else
                TempData["ProductResult"] = "faild";

            return RedirectToAction("GetSuspendProduct");
        }

        #endregion

        #region Edit Detail

        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditProductDetail })]
        public IActionResult EditProduct(int userId, int productId)
        {
            var product = productRepository.GetProductViewModel(userId, productId);
            return View(product);
        }

        [HttpPost]
        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditProductDetail })]
        public IActionResult EditProduct(ProductViewModel productViewModel)
        {
            var result = productRepository.EditProductDetail(productViewModel);
            if (result)
                TempData["EditResult"] = "success";
            else
                TempData["EditResult"] = "faild";

            return RedirectToAction("SearchProduct");
        }

        #endregion
    }
}
