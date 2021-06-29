using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Repositories;


namespace MyFirstEShop.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IProductRepository ProductRepository;
        public ArticleController(IProductRepository _productManager)
        {
            ProductRepository = _productManager;
        }

        public IActionResult Detail(int productId, string productName)
        {
            if (ProductRepository.ExistProduct(productId))
                return View(ProductRepository.GetProductDetail(productId));
            else
                return NotFound();
        }

    }
}
