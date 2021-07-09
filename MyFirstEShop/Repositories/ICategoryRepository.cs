using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Data;
using Microsoft.EntityFrameworkCore;

namespace MyFirstEShop.Repositories
{
    public interface ICategoryRepository
    {
      
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
        Category GetCategory(string categoryName);
        IEnumerable<Product> GetCategoryProducts(string categoryName);
        IEnumerable<HomeViewModel> GetCategoryProductsForView(string categoryName);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext DbContext;
        public CategoryRepository(MyDbContext _dbContext)
        {
            DbContext = _dbContext;
        }
        public IEnumerable<Category> GetCategories()
        {
            return DbContext.Categories.ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return DbContext.Categories.SingleOrDefault(i => i.Id == categoryId);
        }

        public Category GetCategory(string categoryName)
        {
            return DbContext.Categories.SingleOrDefault(i => i.Name == categoryName);
        }

        public IEnumerable<Product> GetCategoryProducts(string categoryName)
        {
            var cat = DbContext.Categories
                .Include(i => i.Products)
                .ThenInclude(i => i.Teacher)
                .ThenInclude(i => i.Info)
                .SingleOrDefault(i => i.Name == categoryName );

            if(cat == null)
            {
                return new List<Product>();
            }
            else
            {
                return cat.Products.Where(i => i.Status == ProductStatus.Active);
            }
            
        }

        public IEnumerable<HomeViewModel> GetCategoryProductsForView(string categoryName)
        {
            var products = GetCategoryProducts(categoryName);

            if(products.Count() == 0)
            {
                return new List<HomeViewModel>();
            }
            else
            {
                return products.Select(i => new HomeViewModel()
                {
                    ProductId = i.Id,
                    ProductName = i.Name,
                    CoverAddress = i.ProductCoverAddress,
                    Price = i.Price,
                    DiscountPersent = i.DiscountPercent,
                    TeacherName = i.Teacher.Info.FirstName + " " + i.Teacher.Info.LastName

                }).ToList();
            }
        }
    }
}
