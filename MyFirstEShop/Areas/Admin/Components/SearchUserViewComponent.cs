using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Areas.Admin.Models.ViewModel;

namespace MyFirstEShop.Areas.Admin.Components
{
    public class SearchUserViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Areas/Admin/Views/UserManagement/SearchUserViewComponent.cshtml", new SearchUserViewModel());
        }

    }
}
