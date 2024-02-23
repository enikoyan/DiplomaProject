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
        [HttpGet]
        [Route("[controller]/login")]
        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated == true)
            {
                return Redirect("/");
            }
            return View();
        }

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _authService.Authenticate(email, password);

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
                //
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return Redirect("/");
        }

        [Route("[controller]/recovery-password")]
        public IActionResult RecoveryPassword()
        {
            return View();
        }
    }
}
