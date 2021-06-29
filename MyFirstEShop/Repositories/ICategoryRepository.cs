using MyFirstEShop.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Data;

namespace MyFirstEShop.Repositories
{
    public interface ICategoryRepository
    {
      
        IEnumerable<Category> GetCategories();
        Category GetCategory(int categoryId);
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
            return DbContext.Categories.First(i => i.Id == categoryId);
        }
    }
}
