using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyFirstEShop.Repositories;


namespace MyFirstEShop.Component
{
    public class ChangeUserInfoViewComponent : ViewComponent
    {

        private readonly IUserRepository userRepository;
        public ChangeUserInfoViewComponent(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            return View("~/Views/ViewComponent/UserSetting/ChangeUserInfo.cshtml", userRepository.GetUserView(userId));
        }

    }
}
