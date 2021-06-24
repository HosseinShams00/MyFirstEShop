using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Data;
using Microsoft.EntityFrameworkCore;

namespace MyFirstEShop.Component
{
    public class TeacherInfoViewComponent : ViewComponent
    {
        private readonly MyDbContext DbContext;
        public TeacherInfoViewComponent(MyDbContext _dbContext)
        {
            DbContext = _dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = int.Parse(UserClaimsPrincipal.FindFirst("UserId").Value);

            var teacher = DbContext.TeacherInfos.Include(i => i.Info).Include(i => i.Products).First(u => u.UserId == userId);

            return View("~/Views/ViewComponent/TeacherPanel/TeacherInfoView.cshtml", teacher);
        }
    }
}
