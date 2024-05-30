using EdManagementSystem.DataAccess.DTO;
using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailClient _emailClient;
        private readonly IConfiguration _configuration;

        public EmailsController(IEmailClient emailClient, IConfiguration configuration)
        {
            _emailClient = emailClient;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult SendEmail(string targetEmail, string message)
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

                bool result = _emailClient.SendMessage(emailConfig, targetEmail, message);

                if (result)
                {
                    return Ok("Сообщение успешно отправлено!");
                }
                else return BadRequest("Не удалось отправить сообщение!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
