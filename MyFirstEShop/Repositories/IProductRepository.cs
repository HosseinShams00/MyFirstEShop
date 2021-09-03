using System.Collections.Generic;
using System.Linq;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Data;
using Microsoft.EntityFrameworkCore;
using MyFirstEShop.Models.ViewModels;
using MyFirstEShop.Areas.Admin.Models.ViewModel;

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
        IEnumerable<Product> GetSuspendProducts();
        int GetSuspendProductsCount();
        IEnumerable<HomeViewModel> GetProducts();
        ProductDetailViewModel GetProductDetail(int productId);
        ProductViewModel GetProductViewModel(int userId, int productId);
        IEnumerable<ProductDetailViewModel> GetProductDetailListForTeacher(int userId);
        void AddNewProduct(ProductViewModel productViewModel, int userId, string productCoverAddress);
        bool EditProductDetail(ProductViewModel productViewModel);
        bool ChangeProductStatus(ProductStatus productStatus, int productId);
        IEnumerable<Product> SearchProduct(SearchProductViewModel searchProductViewModel);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext DbContext;
        private readonly ITeacherRepository teacherRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductRepository(MyDbContext _dbContext, ITeacherRepository _teacherRepository, ICategoryRepository _categoryRepository)
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

        public IEnumerable<Product> GetSuspendProducts()
        {
            return DbContext.Products
                .Where(i => i.Status == ProductStatus.Suspension)
                .ToList();
        }

        public int GetSuspendProductsCount()
        {
            return DbContext.Products
                .Where(i => i.Status == ProductStatus.Suspension)
                .Count();
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

        public ProductViewModel GetProductViewModel(int userId, int productId)
        {
            var checkTeacher = teacherRepository.TeacherHaveThisProduct(userId, productId);
            if (checkTeacher)
            {
                var product = GetProductWhitAllRelation(productId);
                return new ProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CourceLevel = product.ProductOtherInfo.CourceLevel,
                    Create = product.Create,
                    ProductCoverAddress = product.ProductCoverAddress,
                    HaveUpdate = product.HaveUpdate,
                    DiscountPercent = product.DiscountPercent,
                    EndSupport = product.EndSupport,
                    Categories = categoryRepository.GetCategories(),
                    SelectedCategoriesId = product.Categories.Select(i => i.Id).ToArray()
                };
            }
            else
                return null;
        }

        public IEnumerable<ProductDetailViewModel> GetProductDetailListForTeacher(int userId)
        {
            var teacher = teacherRepository.GetTeacherWithProducts(userId);

            if (teacher.Products.Count == 0)
                return new List<ProductDetailViewModel>();

            return teacher.Products
                .Where(i => i.Status != ProductStatus.Inactive)
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

            var Cateories = new List<Category>();

            foreach(var cateoryId in productViewModel.SelectedCategoriesId)
            {
                Cateories.Add(categoryRepository.GetCategory(cateoryId));
            }

            #region Save Product In DB


            var product = new Product()
            {
                Categories = Cateories,
                Name = productViewModel.Name.Trim(),
                Description = productViewModel.Description.Trim(),
                ProductCoverAddress = productCoverAddress,
                Price = productViewModel.Price,
                HaveUpdate = true,
                DiscountPercent = productViewModel.DiscountPercent,
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

        public bool ChangeProductStatus(ProductStatus productStatus, int productId)
        {
            var product = GetProduct(productId);

            if (product == null)
                return false;

            if (productStatus == ProductStatus.Suspension)
            {
                if (product.Status == ProductStatus.Inactive || product.Status == ProductStatus.Suspension)
                {
                    return false;
                }
            }

            product.Status = productStatus;

            DbContext.SaveChanges();

            return true;
        }

        public bool EditProductDetail(ProductViewModel productViewModel)
        {
            var product = GetProductWhitAllRelation(productViewModel.Id.Value);

            product.Name = productViewModel.Name;

            product.Price = productViewModel.Price;

            product.ProductOtherInfo.CourceLevel = productViewModel.CourceLevel;

            product.DiscountPercent = productViewModel.DiscountPercent;

            product.Description = productViewModel.Description;    

            if (productViewModel.SelectedCategoriesId.Length != 0)
            {
                var Cateories = new List<Category>();

                foreach (var cateoryId in productViewModel.SelectedCategoriesId)
                {
                    Cateories.Add(categoryRepository.GetCategory(cateoryId));
                }

                product.Categories = Cateories;
            }
                

            DbContext.SaveChanges();


            return true;
        }

        public IEnumerable<Product> SearchProduct(SearchProductViewModel searchProductViewModel)
        {
            var productQuery = DbContext.Products
                .Include(i => i.Categories)
                .Include(i => i.ProductOtherInfo)
                .Include(i => i.Teacher)
                .ThenInclude(i => i.Info)
                .AsQueryable();

            if (searchProductViewModel.Name != null)
            {
                productQuery = productQuery.Where(p => p.Name.ToLower().Contains(searchProductViewModel.Name.Trim().ToLower()));
            }

            if (searchProductViewModel.TeacherName != null)
            {
                productQuery = productQuery.Where(p => (p.Teacher.Info.FirstName + p.Teacher.Info.LastName).Contains(searchProductViewModel.TeacherName.Trim().ToLower()));
            }

            if (searchProductViewModel.SelectedCategoryId != 0)
            {
                productQuery = productQuery.Where(p => p.Categories.Any(i => i.Id == searchProductViewModel.SelectedCategoryId));
            }

            return productQuery.ToList();
        }
    }
}
