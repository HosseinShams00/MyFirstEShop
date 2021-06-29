using System.Collections.Generic;
using System.Linq;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Data;
using Microsoft.EntityFrameworkCore;
using MyFirstEShop.Models.ViewModels;

namespace MyFirstEShop.Repositories
{
    public interface IProductRepository
    {
        bool ExistProduct(int productId); 
        bool ExistProduct(string productName);
        Product GetProduct(int productId);
        Product GetProductWhitOtherInfo(int productId);
        Product GetProductWhitAllRelation(int productId);
        IEnumerable<Product> GetAllProductWithRelations();
        IEnumerable<HomeViewModel> GetProducts();
        ProductDetailViewModel GetProductDetail(int productId);
        IEnumerable<ProductDetailViewModel> GetProductDetailListForTeacher(int userId);
        void AddNewProduct(ProductViewModel productViewModel,int userId ,string productCoverAddress);
        bool ChangeProductStatus(ProductStatus productStatus, int productId);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext DbContext;
        private readonly ITeacherRepository teacherRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductRepository(MyDbContext _dbContext,ITeacherRepository _teacherRepository,ICategoryRepository _categoryRepository)
        {
            DbContext = _dbContext;
            teacherRepository = _teacherRepository;
            categoryRepository = _categoryRepository;
        }

        public bool ExistProduct(int productId)
        {
            return DbContext.Products.Any(i => i.Id == productId);
        }
       

        public Product GetProduct(int productId)
        {
            return DbContext.Products.SingleOrDefault(i => i.Id == productId);
        }

        public Product GetProductWhitOtherInfo(int productId)
        {
            return DbContext.Products
                .Include(i => i.ProductOtherInfo)
                .SingleOrDefault(i => i.Id == productId);
        }

        public Product GetProductWhitAllRelation(int productId)
        {
            return DbContext.Products
                .Include(i => i.ProductOtherInfo)
                .Include(i => i.Categories)
                .Include(i => i.Teacher)
                .ThenInclude(i => i.Info)
                .SingleOrDefault(i => i.Id == productId);
        }

        public IEnumerable<Product> GetAllProductWithRelations()
        {
           return DbContext
                .Products
                .Include(i => i.ProductOtherInfo)
                .Include(i => i.Teacher)
                .ThenInclude(i => i.Info)
                .ToList();
        }

        public IEnumerable<HomeViewModel> GetProducts()
        {
            return GetAllProductWithRelations()
                .Where(i => i.Status == ProductStatus.Active)
                .Select(i => new HomeViewModel()
                {
                    ProductId = i.Id,
                    ProductName = i.Name,
                    CoverAddress = i.ProductCoverAddress,
                    Price = i.Price,
                    DiscountPersent = i.DiscountPercent,
                    TeacherName = i.Teacher.Info.FirstName + " " + i.Teacher.Info.LastName
                })
                .ToList();
        }

        public ProductDetailViewModel GetProductDetail(int productId)
        {
            var product = GetProductWhitAllRelation(productId);
            if (product == null)
                return null;

            var productDetail = new ProductDetailViewModel()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Price = product.Price,
                CoverAddress = product.ProductCoverAddress,
                CourceLevel = product.ProductOtherInfo.CourceLevel,
                StartDate = product.Create,
                NumberOfPurchases = product.ProductOtherInfo.NumberOfPurchases,
                NumberOfVideos = product.ProductOtherInfo.NumberOfVideos,
                TeacherName = product.Teacher.Info.FirstName + " " + product.Teacher.Info.LastName

            };

            return productDetail;
        }

        public IEnumerable<ProductDetailViewModel> GetProductDetailListForTeacher(int userId)
        {
            var teacher = teacherRepository.GetTeacherWithProducts(userId);

            return teacher.Products
                .Select(i => new ProductDetailViewModel()
                {
                    ProductId = i.Id,
                    ProductName = i.Name,
                    Description = i.Description,
                    CoverAddress = i.ProductCoverAddress,
                    Price = i.Price,
                    Status = i.Status,
                    CourceLevel = i.ProductOtherInfo.CourceLevel,
                    NumberOfPurchases = i.ProductOtherInfo.NumberOfPurchases,
                    NumberOfVideos = i.ProductOtherInfo.NumberOfVideos,
                    StartDate = i.Create,
                    TeacherName = i.Teacher.Info.FirstName + " " + i.Teacher.Info.LastName
                })
                .ToList();
        }

        public bool ExistProduct(string productName)
        {
            return DbContext.Products
                .Any(i => i.Name.ToLower() == productName.Trim().ToLower());
        }

        public void AddNewProduct(ProductViewModel productViewModel, int userId, string productCoverAddress)
        {
            var teacher = teacherRepository.GetTeacher(userId);

            var category = categoryRepository.GetCategory(productViewModel.CategoryId);

            #region Save Product In DB


            var product = new Product()
            {
                Categories = new List<Category>() { category },
                Name = productViewModel.Name.Trim(),
                Description = productViewModel.Description.Trim(),
                ProductCoverAddress = productCoverAddress,
                Price = productViewModel.Price,
                HaveUpdate = true,
                DiscountPercent = productViewModel.OffPercent,
                TeacherId = teacher.Id,
                Create = System.DateTime.Now,
                EndSupport = null,

            };

            DbContext.Add(product);
            DbContext.SaveChanges();


            #endregion

            #region Add Other Detail To Product

            var productOtherInfo = new ProductOtherInfo()
            {
                ProdutId = product.Id,
                CourceLevel = productViewModel.CourceLevel,
                TotalTime = System.DateTime.Now

            };

            DbContext.Add(productOtherInfo);
            DbContext.SaveChanges();

            #endregion


            DbContext.SaveChanges();
        }

        public bool ChangeProductStatus(ProductStatus productStatus,int productId)
        {
            var product = GetProduct(productId);

            if (product == null)
                return false;

            product.Status = ProductStatus.Suspension;

            DbContext.SaveChanges();

            return true;
        }
    }
}
