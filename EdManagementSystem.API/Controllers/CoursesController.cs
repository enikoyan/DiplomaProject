using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            try
            {
                var course = await _courseService.GetCourseById(courseId);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{tutorId}")]
        public async Task<IActionResult> GetCoursesByTutor(int tutorId)
        {
            try
            {
                var courses = await _courseService.GetCoursesByTutor(tutorId);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{courseName}")]
        public async Task<IActionResult> GetCourseByName(string courseName)
        {
            try
            {
                var course = await _courseService.GetCourseByName(courseName);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourseById(int courseId)
        {
            await _courseService.DeleteCourse(courseId);
            return NoContent();
        }

        [HttpDelete("{courseName}")]
        public async Task<IActionResult> DeleteCourseByName(string courseName)
        {
            await _courseService.DeleteCourse(courseName);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCourses();
            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            try
            {
                var newCourse = await _courseService.CreateCourse(course);
                return CreatedAtAction(nameof(GetCourseById), new { courseId = newCourse.CourseId }, newCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("{courseName}")]
        public async Task<IActionResult> GetCourseIdByName(string courseName)
        {
            try
            {
                var courseId = await _courseService.GetCourseIdByName(courseName);
                return Ok(courseId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
