using EdManagementSystem.DataAccess.DTO;
using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRecoveryService _recoveryService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IRecoveryService recoveryService, IConfiguration configuration)
        {
            _authService = authService;
            _recoveryService = recoveryService;
            _configuration = configuration;
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

        [HttpGet]
        [ActionName("getRecoveryKey")]
        public async Task<IActionResult> RecoveryTrigger(string userEmail)
        {
            try
            {
                EmailConfigDTO emailConfig = new EmailConfigDTO()
                {
                    SmtpServer = _configuration["EmailConfiguration:SmtpServer"]!,
                    SmtpPassword = _configuration["EmailConfiguration:SmtpPassword"]!,
                    SmtpUsername = _configuration["EmailConfiguration:SmtpUsername"]!,
                    SmtpPort = Int32.Parse(_configuration["EmailConfiguration:SmtpPort"]!),
                };

                var result = await _recoveryService.RecoveryTrigger(userEmail, emailConfig);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("changePassword")]
        public async Task<IActionResult> ChangePassword(Guid serverKey, Guid clientKey, string newPassword)
        {
            var result = await _recoveryService.RecoveryProcess(serverKey, clientKey, newPassword);

            if (result)
            {
                return Ok("Пароль успешно сменен! Используйте его для входа.");
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
