using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using MyFirstEShop.Repositories;

namespace MyFirstEShop.Attributes
{
    class CheckUserSecurityStampAttribute : ActionFilterAttribute
    {
        private readonly IUserRepository userRepository;
        public CheckUserSecurityStampAttribute(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(context.HttpContext.User.FindFirst("UserId").Value);

                var securityStamp = Guid.Parse(context.HttpContext.User.FindFirst("SecurityStamp").Value);

                var user = userRepository.GetUser(userId);

                if (securityStamp != user.SecurityStamp)
                {
                    context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        }

        
    }
}
