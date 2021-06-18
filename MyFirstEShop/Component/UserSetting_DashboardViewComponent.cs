using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Data;
using MyFirstEShop.Models.ViewModels;

namespace MyFirstEShop.Component
{
    public class UserSetting_DashboardViewComponent : ViewComponent
    {
        public MyDbContext DbContext { get; set; }
        public UserSetting_DashboardViewComponent(MyDbContext _dbContext)
        {
            DbContext = _dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            var selectUser = DbContext.UserInfos.SingleOrDefault(i => i.Id == userId);

            var ComponentModel = new UserViewModel()
            {
                FirstName = selectUser.FirstName,
                LastName = selectUser.LastName,
                EmailAddress = selectUser.Email,
                PhoneNumber = selectUser.PhoneNumber,
                About = selectUser.About,
                Address = selectUser.Address,
                RegisterTime = selectUser.RegisterTime
            };

            return View("~/Views/ViewComponent/UserSetting/Dashboard.cshtml",ComponentModel);
        }
    }
}
