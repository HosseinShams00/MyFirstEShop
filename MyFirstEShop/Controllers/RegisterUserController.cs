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
                    UserRegister.AddUser(new UserInfo()
                    {
                        FirstName = registerAccount.FirstName,
                        LastName = registerAccount.LastName,
                        Email = registerAccount.EmailAddress,
                        Password = registerAccount.Password,
                        UserName = registerAccount.UserName,
                        IsAdmin = false,
                        RegisterTime = DateTime.Now
                    });
                    return Content("Success");
                }
            }
            else
            {
                return View(registerAccount);
            }
        }

        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModelView loginModel)
        {
            if (ModelState.IsValid)
            {
                var User = UserRegister.GetUserByEmail(loginModel.Email.ToLower().Trim());
                if (User != null && User.Password == loginModel.Password)
                {
                    var claims = new List<Claim>()
                    {
                         new Claim("Userid" , User.Id.ToString()),
                         new Claim("FirstName" , User.FirstName),
                         new Claim("LastName" , User.LastName == null ? " " : User.LastName ),
                         new Claim("Email" , loginModel.Email.Split('@')[0]),
                    };

                    var ClaimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var Principal = new ClaimsPrincipal(ClaimIdentity);

                    var prop = new AuthenticationProperties
                    {
                        IsPersistent = loginModel.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, Principal, prop);

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


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
