using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MyFirstEShop.Models.DatabaseModels;
using MyFirstEShop.Models.ViewModels;
using System.Text;

namespace MyFirstEShop.Controllers
{
    public class RegisterUserController : Controller
    {
        private readonly IUserRepository UserRepository;
        private readonly IUserSecurityRepository userSecurityRepository;
        private readonly IMessageSenderRepository messageSenderRepository;
        private readonly IHasher hasher;
        public RegisterUserController(IUserRepository userRegisterRepositry, IMessageSenderRepository _messageSenderRepository, IHasher _hasher, IUserSecurityRepository _userSecurityRepository)
        {
            UserRepository = userRegisterRepositry;
            userSecurityRepository = _userSecurityRepository;
            messageSenderRepository = _messageSenderRepository;
            hasher = _hasher;
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
                if (UserRepository.ExistUser(registerAccount.EmailAddress.ToLower().Trim()))
                {
                    ModelState.AddModelError("EmailAddress", "شخصی با همچین مشخصات وجود دارد");
                    return View(registerAccount);
                }
                else
                {
                    try
                    {
                        UserRepository.AddUser(registerAccount);
                        SendVerifyEmail(registerAccount.EmailAddress.ToLower().Trim());

                        TempData["ProcessDetail"] = "success-VerifyEmail";
                        return RedirectToAction("Login");

                    }
                    catch (Exception ex)
                    {
                        TempData["ProcessDetail"] = "error";
                        return RedirectToAction("Login");
                    }

                }
            }
            else
            {
                return View(registerAccount);
            }
        }

        private void SendVerifyEmail(string email)
        {
            var token = userSecurityRepository.SetEmailVerifyToken(email);
            StringBuilder sb = new StringBuilder();
            string h2Style = "style=\"display: block;direction: rtl;text-align: center;color: black; \" ";
            sb.AppendLine($"<h2 {h2Style}>با سلام کاربر محترم لطفا جهت فعال سازی ایمیل خود بر روی لینک زیر کلیک نماید</h2>");

            string aStyle = "style=\" color: white;text-decoration: none;display: block;text-align: center;width: 60%;background-color: #1D8348;margin: 0 auto; font-size: 1.3rem;border-radius: 50px;border: 1px black solid;padding: 4vh 0;\"";

            sb.AppendLine($"<a {aStyle} href=\"{Url.Action("VerifyEmail", "RegisterUser", new { token = token, email = email }, Request.Scheme)}\" >کلیک کنید</a>");


            messageSenderRepository.SendEmail(email, "ثبت ایمیل", sb.ToString(), true);
        }

        #endregion

        #region Login

        public IActionResult Login(string url)
        {
            ViewBag.ProcessDetail = TempData["ProcessDetail"];
            return View(new LoginViewModel() { RedirectUrl = url });
        }

        [HttpPost] 
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var User = UserRepository.GetUser(loginModel.Email.ToLower().Trim());
                if (User != null && UserRepository.CheckUserPasssword(loginModel.Password, User.Id))
                {
                    if (User.VerifyEmail)
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim("UserId" , User.Id.ToString()),
                            new Claim("FirstName" , User.FirstName),
                            new Claim("LastName" , User.LastName == null ? " " : User.LastName ),
                            new Claim("IsTeacher" , User.IsTeacher.ToString()),
                            new Claim("IsAdmin" , User.IsAdmin.ToString()),
                            new Claim("SecurityStamp" , User.SecurityStamp != null ? User.SecurityStamp.ToString() : " "),
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
                        SendVerifyEmail(loginModel.Email.ToLower().Trim());
                        TempData["ProcessDetail"] = "Check-EmilVerify";
                        return RedirectToAction("Login");
                    }
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
            int userId = int.Parse(User.FindFirst("UserId").Value);
            Guid stamp = Guid.Parse(User.FindFirst("SecurityStamp").Value);
            if (userSecurityRepository.VerifyUserStamp(userId, stamp))
            {
                UserRepository.ChangeInfo(user);
                return RedirectToAction("Index", "Setting");
            }
            else
            {
                return RedirectToAction("Logout");
            }
        }


        #region Reset Password

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = UserRepository.GetUser(resetPasswordViewModel.Email);

            if (user != null)
            {
                var password = hasher.GenerateToken();

                password = userSecurityRepository.SetPasswordRecovery(user.Email, password);

                StringBuilder sb = new StringBuilder();

                string h1Style = "style=\"display: block;direction: rtl;text-align: center;color: black; \" ";
                sb.AppendLine($"<h1 {h1Style} >سلام لطفا برای بازیابی پسورد خود بر روی دکمه زیر کلیک نمایید .</h1>");

                string aStyle = "style=\" color: white;text-decoration: none;display: block;text-align: center;width: 60%;background-color: #1D8348;margin: 0 auto;" +
                    "font-size: 1.3rem;border-radius: 50px;border: 1px black solid;padding: 5vh 0;\"";

                sb.AppendLine($"<a {aStyle} href=\"{Url.Action("SetNewPassword", "RegisterUser", new { token = password, email = user.Email }, Request.Scheme)}\" >کلیک کنید</a>");

                messageSenderRepository.SendEmail(user.Email, "ریست کردن پسورد", sb.ToString(), true);

                TempData["ProcessDetail"] = "success-ResetPassword";
                return RedirectToAction("Login");
            }
            else
            {
                return Content("خطایی به وجود امده");
            }
        }



        #endregion

        #region Set New Password

        public IActionResult SetNewPassword(string token, string email)
        {
            try
            {
                userSecurityRepository.CheckPasswordRecoveryToken(email, token);
                return View(new ResetPasswordViewModel()
                {
                    Email = email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return View(new ResetPasswordViewModel() { Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetNewPassword(ResetPasswordViewModel viewModel)
        {
            userSecurityRepository.ChangePassword(viewModel.Email, viewModel.NewPassword);

            /// we should redirect to logout with Post Method

            if (User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        #endregion

        public IActionResult VerifyEmail(string token, string email)
        {
            if (userSecurityRepository.VerifyEmail(email, token))
            {
                TempData["ProcessDetail"] = "success-VerifyEmail";
                return RedirectToAction("Login");
            }
            else
            {
                return NotFound();
            }

        }
    }
}
