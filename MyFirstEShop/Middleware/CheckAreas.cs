using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstEShop.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CheckAreas
    {
        private readonly RequestDelegate _next;

        public CheckAreas(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var adminSegment = httpContext.Request.Path.StartsWithSegments("/admin");
            var teacherSegment = httpContext.Request.Path.StartsWithSegments("/teacher");

            if (adminSegment)
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    if (bool.Parse(httpContext.User.FindFirst("IsAdmin").Value) == false)
                    {
                        httpContext.Response.Redirect("/Error/404");
                    }
                    else
                        await _next(httpContext);
                }
                else
                {
                    httpContext.Response.Redirect("/Error/404");
                }
            }
            else
            {
                if (teacherSegment)
                {
                    if (httpContext.User.Identity.IsAuthenticated)
                    {
                        if (bool.Parse(httpContext.User.FindFirst("IsTeacher").Value) == false)
                        {
                            httpContext.Response.Redirect("/Error/404");
                        }
                    
                        else
                            await _next(httpContext);
                    }
                    else
                    {

                        httpContext.Response.StatusCode = 404;
                    }
                }
                else
                    await _next(httpContext);
            }
 
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CheckAreasExtensions
    {
        public static IApplicationBuilder UseCheckAreas(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckAreas>();
        }
    }
}
