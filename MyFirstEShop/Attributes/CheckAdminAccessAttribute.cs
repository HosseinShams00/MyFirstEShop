using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using MyFirstEShop.Models.DatabaseModels;
using Microsoft.Extensions.Logging;

namespace MyFirstEShop.Attributes
{

    public class CheckAdminAccessAttribute : ActionFilterAttribute
    {

        private readonly IUserAccessRepository userAccessRepository;
        private readonly Access checkAccess;

        public CheckAdminAccessAttribute(IUserAccessRepository _userAccessRepository, Access _checkAccess)
        {
            userAccessRepository = _userAccessRepository;
            checkAccess = _checkAccess;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(context.HttpContext.User.FindFirst("UserId").Value);
                var adminAccess = userAccessRepository.GetUserAccess(userId);
                if (adminAccess.Count() != 0)
                {
                    var result = adminAccess.Any(i => i == checkAccess);
                    if (result == false)
                    {
                        context.HttpContext.Response.Redirect("/Error/404");
                    }
                }
                else
                {
                    context.HttpContext.Response.Redirect("/Error/404");
                }
            }



        }


    }
}
