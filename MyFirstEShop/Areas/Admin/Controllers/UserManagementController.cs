using Microsoft.AspNetCore.Mvc;
using MyFirstEShop.Areas.Admin.Models.ViewModel;
using MyFirstEShop.Attributes;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Repositories;
using System.Collections.Generic;

namespace MyFirstEShop.Areas.Admin.Controllers
{

    [Area("Admin")]
    [TypeFilter(typeof(CheckUserSecurityStampAttribute))]
    public class UserManagementController : Controller
    {

        private readonly IUserRepository userRepository;
        private readonly IUserAccessRepository userAccessRepository;

        public int AdminUserId
        {
            get
            {
                return int.Parse(User.FindFirst("UserId").Value);
            }

        }

        public UserManagementController(IUserRepository _userRepository, IUserAccessRepository _userAccessRepository)
        {
            userRepository = _userRepository;
            userAccessRepository = _userAccessRepository;
        }

        #region Search

        // Check Admin Have Access To Edit User Info And Search Him
        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditUserInfo })]
        public IActionResult Search()
        {
            return View(new List<User>());
        }

        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditUserInfo })]
        public IActionResult Result(SearchUserViewModel searchUserViewModel)
        {
            var user = userRepository.SearchUser(searchUserViewModel);
            return View("Search", user);
        }

        #endregion

        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanSetAccessForUser })]
        public IActionResult EditUserAccess(int userId)
        {
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanSetAccessForUser })]
        public IActionResult EditUserAccess()
        {
            return View();
        }


        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditUserInfo })]
        public IActionResult EditUserInfo(int userId)
        {
            var adminAccess = userAccessRepository.GetUserAccess(AdminUserId);
            var userAccess = userAccessRepository.GetUserAccess(userId);
            var user = userRepository.GetUser(userId);
            user.Password = null;

            var ViewModel = new EditUserInfoViewModel()
            {
                AdminAccess = adminAccess,
                UserAccess = userAccess,
                User = user,
            };

            return View(ViewModel);
        }

        [HttpPost]
        [TypeFilter(typeof(CheckAdminAccessAttribute), Arguments = new object[] { Access.CanEditUserInfo })]
        public IActionResult EditUserInfo(EditUserInfoViewModel editUserInfoViewModel)
        {
            editUserInfoViewModel.AdminAccess = userAccessRepository.GetUserAccess(AdminUserId);
            userAccessRepository.SetUserAccess(editUserInfoViewModel);

            return RedirectToAction("Search");
        }
    }
}
