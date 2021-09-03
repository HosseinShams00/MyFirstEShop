using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Attributes
{
    public class CheckCoockieValueAttribute : ActionFilterAttribute
    {

        private readonly string CoockieValue;

        public CheckCoockieValueAttribute(string Value)
        {
            CoockieValue = Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (bool.Parse(context.HttpContext.User.FindFirst(CoockieValue).Value) == false)
                {
                    context.HttpContext.Response.Redirect("/Error/404");
                }
            }
        }
    }
}
