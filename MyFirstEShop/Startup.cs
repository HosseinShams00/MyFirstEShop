using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstEShop.Models;
using MyFirstEShop.Middleware;
using MyFirstEShop.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyFirstEShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            #region DbConfig

            services.AddDbContext<Data.MyDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MyContext"));
            });

            #endregion

            #region Repository

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserSecurityRepository, UserSecurityRepository>();
            services.AddScoped<IUserAccessRepository, UserAccessRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ICartRepository, ShopManager>();
            services.AddScoped<IMessageSenderRepository, MessageSenderRepository>();
            services.AddScoped<IHasher, Hash>();
            services.AddScoped<ITicketRepository , TicketRepository>();

            #endregion

            #region Authentication

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/RegisterUser/Login";
                options.LogoutPath = "/RegisterUser/Logout";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
        
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCheckAreas();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Article",
                    pattern: "Article/Detail/{productId}/{productName?}",
                    defaults: new { controller = "Article" , action = "Detail" });

                endpoints.MapControllerRoute(
                    name: "Admin", 
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
