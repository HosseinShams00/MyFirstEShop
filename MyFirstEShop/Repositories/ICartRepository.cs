using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFirstEShop.Data;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;

namespace MyFirstEShop.Repositories
{
    public interface ICartRepository
    {
        void AddProductToCart(int productId, int userId);
        void RemoveProduct(int productId, int userId);
        bool HaveCart(int userId);
        Cart GetCart(int userId);
        Cart GetCartFull(int userId);
        CartViewModel GetCartViewModel(int userId);
    }

    public class ShopManager : ICartRepository
    {
        public MyDbContext DbContext { get; set; }
        public IProductRepository ProductManager { get; set; }
        public IUserRepository UserManager { get; set; }

        public ShopManager(MyDbContext _dbContext, IProductRepository _productMg, IUserRepository _userManager)
        {
            DbContext = _dbContext;
            ProductManager = _productMg;
            UserManager = _userManager;
        }

        public void AddProductToCart(int productId, int userId)
        {
            var product = ProductManager.GetProduct(productId);
            var user = UserManager.GetUser(userId);
            var cart = DbContext.Cart.SingleOrDefault(i => i.UserId == userId);

            if (cart == null)
            {
                DbContext.Cart.Add(new Cart()
                {
                    Products = { product },
                    User = user,
                });
                DbContext.SaveChanges();
            }
            else
            {
                if (!(cart.Products.Any(i => i.Id == product.Id)))
                {
                    cart.Products.Add(product);
                }
                DbContext.SaveChanges();
            }


        }

        public void RemoveProduct(int productId, int userId)
        {
            var cart = GetCart(userId);
            var checkProductInCart = cart.Products.Any(i => i.Id == productId);
            if (checkProductInCart)
            {
                cart.Products.Remove(ProductManager.GetProduct(productId));
            }
            DbContext.SaveChanges();
        }
        public bool HaveCart(int userId)
        {
            return DbContext.Cart.Any(i => i.UserId == userId);
        }

        public Cart GetCart(int userId)
        {
            return DbContext.Cart.Include(i => i.Products).SingleOrDefault(i => i.UserId == userId);
        }
        public Cart GetCartFull(int userId)
        {
            return DbContext.Cart
                .Include(i => i.Products)
                .ThenInclude(i => i.Teacher)
                .ThenInclude(i => i.Info)
                .Include(i => i.User)
                .SingleOrDefault(i => i.UserId == userId);
        }

        public CartViewModel GetCartViewModel(int userId)
        {

            if (HaveCart(userId))
            {
                var cart = GetCartFull(userId);
                var cartItems = new List<CartItemViewModel>();


                foreach (var item in cart.Products)
                {
                    cartItems.Add(new CartItemViewModel()
                    {
                        ProductId = item.Id,
                        ProductName = item.Name,
                        ProductCoverAddress = item.ProductCoverAddress,
                        Price = item.Price,
                        TeacherName = item.Teacher.Info.FirstName + " " + item.Teacher.Info.LastName,
                        DiscountPrice = item.DiscountPercent
                    });
                }
                return new CartViewModel()
                {
                    CartId = cart.CartId,
                    Items = cartItems
                };
            }
            else
            {
                return null;
            }
        }

        
    }
}
