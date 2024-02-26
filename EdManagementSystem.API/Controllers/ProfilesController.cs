using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/profile/[action]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileInfoService _profileService;

        public ProfilesController(IProfileInfoService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{teacherEmail}")]
        public async Task<IActionResult> GetSquadsCount(string teacherEmail)
        {
            try
            {
                int squadsCount = await _profileService.GetSquadsCount(teacherEmail);
                return Ok(squadsCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{teacherEmail}")]
        public async Task<IActionResult> GetStudentsCount(string teacherEmail)
        {
            try
            {
                int studentsCount = await _profileService.GetStudentsCount(teacherEmail);
                return Ok(studentsCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{teacherEmail}")]
        public async Task<IActionResult> GetCoursesOfTeacher(string teacherEmail)
        {
            var courses = await _profileService.GetCoursesOfTeacher(teacherEmail);
            return Ok(courses);
        }

        [HttpGet]
        [Route("{teacherEmail}")]
        public async Task<IActionResult> GetSquadsOfTeacher(string teacherEmail)
        {
            var squads = await _profileService.GetSquadsOfTeacher(teacherEmail);
            return Ok(squads);
        }
    }
}
