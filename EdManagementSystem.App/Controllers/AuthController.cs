using EdManagementSystem.App.Models;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Text.Json;

namespace EdManagementSystem.App.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMemoryCache _memoryCache;

        public AuthController(IAuthService authService, IMemoryCache memoryCache)
        {
            _authService = authService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [Route("auth/login")]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated == true)
            {
                if (User.IsInRole("admin"))
                {
                    return Redirect("/admin-panel");
                }
                else return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _authService.Authenticate(model.Email, model.Password);

            if (user == null)
            {
                ViewBag.ErrorEmail = "Неправильный email или пароль!";
                return View();
            }

            var userEmail = _memoryCache.Get<string>("UserEmail");

            if (userEmail != user.UserEmail)
            {
                _memoryCache.Remove("UserEmail");
                _memoryCache.Remove("profileData");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserEmail),
                new Claim(ClaimTypes.Role, user.UserRole),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (User.IsInRole("admin"))
            {
                return Redirect("admin-panel");
            }
            else
            {
                _memoryCache.Set("UserEmail", user.UserEmail);
                return Redirect("/");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //_memoryCache.Remove("profileData");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/auth/login");
        }

        [Route("[controller]/recovery-password")]
        public IActionResult RecoveryPassword()
        {
            return View();
        }
    }
}
