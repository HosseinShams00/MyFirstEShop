using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Component
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryViewComponent(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Views/ViewComponent/CategoryViewComponent.cshtml", categoryRepository.GetCategories());
        }

    }
}
