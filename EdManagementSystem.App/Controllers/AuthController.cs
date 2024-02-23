using EdManagementSystem.App.Models;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EdManagementSystem.App.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("[controller]/login")]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated == true)
            {
                return Redirect("/");
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

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
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
