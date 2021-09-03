using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Areas.Admin.Models.ViewModel;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Areas.Admin.Components
{
    public class SearchProductViewComponent : ViewComponent
    {
        private readonly ICategoryRepository categoryRepository;
        public SearchProductViewComponent(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Areas/Admin/Views/ProductManagement/SearchProductComponentView.cshtml" , new SearchProductViewModel() { Categories = categoryRepository.GetCategories().ToList()});
        }

    }
}
