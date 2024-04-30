using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using EdManagementSystem.DataAccess.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeworksController : ControllerBase
    {
        private readonly IHomeworkService _homeworkService;

        public HomeworksController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHomework([FromForm] string groupBy, [FromForm] List<string> foreignKeys,
            [FromForm]string title, [FromForm] string? description, [FromForm] string? note, [FromForm] DateTime? deadline)
        {
            if (string.IsNullOrWhiteSpace(groupBy) || string.IsNullOrWhiteSpace(title) || foreignKeys.Count == 0)
            {
                throw new ArgumentException("Заполните все необходимые данные!");
            }
            try
            {
                bool result = await _homeworkService.CreateHomework(groupBy, foreignKeys, title, description, note, deadline);

                if (result)
                {
                    return Ok("Домашние задания успешно созданы!");
                }
                else
                {
                    return BadRequest("Не удалось создать Д/З!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Возникла ошибка: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHomeworks()
        {
            var homeworks = await _homeworkService.GetAllHomeworks();
            return Ok(homeworks);
        }

        [HttpGet("{courseName}")]
        public async Task<IActionResult> GetHomeworksByCourse(string courseName)
        {
            var homeworks = await _homeworkService.GetHomeworksByCourse(courseName);
            return Ok(homeworks);
        }

        [HttpGet("{squadName}")]
        public async Task<IActionResult> GetHomeworksBySquad(string squadName)
        {
            var homeworks = await _homeworkService.GetHomeworksBySquad(squadName);
            return Ok(homeworks);
        }

        [HttpGet("{title}")]
        public async Task<IActionResult> GetHomeworksByTitle(string title)
        {
            var homeworks = await _homeworkService.GetHomeworksByTitle(title);
            return Ok(homeworks);
        }

        [HttpGet("{homeworkId}")]
        public async Task<IActionResult> GetHomeworkById(Guid homeworkId)
        {
            var homeworks = await _homeworkService.GetHomeworkById(homeworkId);
            return Ok(homeworks);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteHomeworkById(Guid homeworkId)
        {
            try
            {
                bool result = await _homeworkService.DeleteHomework(homeworkId);
                if (result) { return Ok("Домашнее задание успешно удалено!"); }
                else { return BadRequest("Не удалось удалить Д/З!"); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
