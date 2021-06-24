using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Models;
using MyFirstEShop.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MyFirstEShop.Models.ViewModels;


namespace MyFirstEShop.Controllers
{
    public class RegisterUserController : Controller
    {
        public IUserRegisterRepository UserRegister { get; set; }
        public RegisterUserController(IUserRegisterRepository userRegisterRepositry)
        {
            UserRegister = userRegisterRepositry;
        }
        

        #region Signin

        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signin(RegisterAccountModelView registerAccount)
        {
            if (ModelState.IsValid)
            {
                if (UserRegister.ExistUser(registerAccount.EmailAddress.ToLower().Trim()))
                {
                    ModelState.AddModelError("EmailAddress", "شخصی با همچین مشخصات وجود دارد");
                    return View(registerAccount);
                }
                else
                {
                    UserRegister.AddUser(new User()
                    {
                        FirstName = registerAccount.FirstName,
                        LastName = registerAccount.LastName,
                        Email = registerAccount.EmailAddress,
                        Password = registerAccount.Password,
                        IsAdmin = false,
                        RegisterTime = DateTime.Now
                    });
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return View(registerAccount);
            }
        }

        #endregion

        #region Login

        public IActionResult Login(string url)
        {
            return View(new LoginViewModel() { RedirectUrl = url });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var User = UserRegister.GetUserByEmail(loginModel.Email.ToLower().Trim());
                if (User != null && User.Password == loginModel.Password)
                {
                    var claims = new List<Claim>()
                    {
                         new Claim("UserId" , User.Id.ToString()),
                         new Claim("FirstName" , User.FirstName),
                         new Claim("LastName" , User.LastName == null ? " " : User.LastName ),
                         new Claim("Email" , User.Email),
                         new Claim("IsTeacher" , User.IsTeacher.ToString()),
                         new Claim("IsAdmin" , User.IsAdmin.ToString()),

                    };

                    var ClaimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var Principal = new ClaimsPrincipal(ClaimIdentity);

                    var prop = new AuthenticationProperties
                    {
                        IsPersistent = loginModel.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal, prop);

                    if (loginModel.RedirectUrl != null)
                        return LocalRedirect(loginModel.RedirectUrl);
                    else
                        return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("Email", "شخصی با چنین مشخصاتی یافت نشد");
                    return View(loginModel);
                }



            }
            else
            {
                return View(loginModel);
            }

        }

        #endregion

        #region Logout

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        #endregion

        [Authorize]
        [HttpPost]
        public IActionResult ChangeInfo(UserViewModel user)
        {
            var UpdateUser = new User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                About = user.About,
                Address = user.Address
            };

            UserRegister.ChangeInfo(UpdateUser);
            return RedirectToAction("Index", "Setting");
        }
    }
}
