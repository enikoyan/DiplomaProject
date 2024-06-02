using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SocialMediaController : ControllerBase
    {
        private readonly ISocialMediaService _socialMediaService;

        public SocialMediaController(ISocialMediaService socialMediaService)
        {
            _socialMediaService = socialMediaService;
        }

        [HttpPost]
        public async Task<ActionResult> AddSocialMedium([FromBody] SocialMedium socialMediumDto)
        {
            try
            {
                await _socialMediaService.AddSocialMedium(socialMediumDto.IdTeacher, socialMediumDto.SocialMediaName, socialMediumDto.SocialMediaUrl);
                return Ok("Социальная сеть успешно добавлена");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{teacherEmail}")]
        public async Task<IActionResult> GetSocialMedia(string teacherEmail)
        {
            try
            {
                var socialMediaItems = await _socialMediaService.GetSocialMediaById(teacherEmail);
                return Ok(socialMediaItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
