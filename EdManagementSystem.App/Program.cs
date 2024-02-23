using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Infrastructure;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EdManagementSystem.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var builderConfig = builder.Configuration;

            services.AddControllersWithViews();

            #region Authentication

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/login";
                    options.Cookie.Name = "EdManagementSystemCookie";
                });

            // Get DB connection and DBContext
            var connectionString = builderConfig.GetConnectionString("DefaultConnection");
            services.AddDbContext<User004Context>(dbContextOptions => dbContextOptions
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString) ??
            throw new InvalidOperationException("Connection string is not found!")),
            ServiceLifetime.Singleton);

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            #endregion

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
