using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MyFirstEShop.Controllers
{
    [Authorize]
    public class ShopingListController : Controller
    {
        private readonly ICartRepository CartRepository;
        private readonly IProductRepository ProductRepository;
        private int UserId
        {
            get
            {
                return int.Parse(User.FindFirst("UserId").Value);
            }
        }

        public ShopingListController(ICartRepository _shopManager, IProductRepository _productManager)
        {
            CartRepository = _shopManager;
            ProductRepository = _productManager;
        }

        public IActionResult Index()
        {
            return View(CartRepository.GetCartViewModel(UserId));
        }

        //[HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = ProductRepository.ExistProduct(productId);

            if (product)
            {
                CartRepository.AddProductToCart(productId, UserId);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            CartRepository.RemoveProduct(productId, UserId);
            return RedirectToAction("Index");
        }
    }
}
