using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ActionName("login")]
        public IActionResult Login(string login, string password)
        {
            var user = _authService.Authenticate(login, password);

            if (user == null)
            {
                return Unauthorized(new { error = "Неправильный email или пароль!" });
            }

            // Возвращаем данные пользователя вместе с успешным статусом
            return Ok(new { message = "Успешная аутентификация", user });
        }
    }
}
