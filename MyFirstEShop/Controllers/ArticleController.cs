using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Repositories;


namespace MyFirstEShop.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IProductRepository ProductRepository;
        private readonly ICategoryRepository categoryRepository;
        public ArticleController(IProductRepository _productManager , ICategoryRepository _categoryRepository)
        {
            ProductRepository = _productManager;
            categoryRepository = _categoryRepository;
        }

        public IActionResult Detail(int productId, string productName)
        {
            if (ProductRepository.ExistProduct(productId))
                return View(ProductRepository.GetProductDetail(productId));
            else
                return NotFound();
        }

        public IActionResult CategoryProducts(string categoryName)
        {
            var products = categoryRepository.GetCategoryProductsForView(categoryName);
            return View(products);
        }

    }
}
