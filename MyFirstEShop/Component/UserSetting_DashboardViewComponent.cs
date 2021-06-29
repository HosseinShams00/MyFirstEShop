using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Component
{
    public class UserSetting_DashboardViewComponent : ViewComponent
    {
        private readonly IUserRepository userRepository;
        public UserSetting_DashboardViewComponent(IUserRepository _userRepository )
        {
            userRepository = _userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            return View("~/Views/ViewComponent/UserSetting/Dashboard.cshtml", userRepository.GetUserView(userId));
        }
    }
}
