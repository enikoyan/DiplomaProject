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
        private readonly IEmailClient _emailClient;

        public AuthController(IAuthService authService, IRecoveryService recoveryService, IConfiguration configuration, IEmailClient emailClient)
        {
            _authService = authService;
            _recoveryService = recoveryService;
            _configuration = configuration;
            _emailClient = emailClient;
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
        public async Task<IActionResult> ChangePassword([FromForm] Guid serverKey, [FromForm] Guid clientKey, [FromForm] string newPassword)
        {
            var result = await _recoveryService.RecoveryProcess(serverKey, clientKey, newPassword);

            if (result != null)
            {
                SendAnswerEmail(result);
                return Ok("Пароль успешно сменен! Используйте его для входа.");
            }
            else
            {
                return BadRequest(result);
            }
        }

        // Send email when password has changed
        private bool SendAnswerEmail(string userEmail)
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

                var message = $"<h1>Уважаемый пользователь, ваш пароль бьл изменен!</h1></br>" +
    $"<p>Если это были не вы, сообщите нам в тех поддержку: <a target=\"blank\">https://vk.com/nikoyan4</a></p>";

                _emailClient.SendMessage(emailConfig, userEmail, message);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
