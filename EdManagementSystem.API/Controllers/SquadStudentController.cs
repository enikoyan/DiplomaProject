using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SquadStudentController : ControllerBase
    {
        private readonly ISquadStudentService _squadStudentService;

        public SquadStudentController(ISquadStudentService squadStudentService)
        {
            _squadStudentService = squadStudentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSquadStudent(int studentId, int squadId)
        {
            var success = await _squadStudentService.CreateSquadStudent(studentId, squadId);

            if (success)
            {
                return Ok("Студент успешно прикреплен к группе!");
            }
            else
            {
                return BadRequest(error: "Не удалось прикрепить студента к группе, либо он уже прикреплен!");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<int>>> GetStudentsIdsBySquads([FromQuery]List<int> squadsIds)
        {
            try
            {
                var studentIds = await _squadStudentService.GetStudentsIdsBySquads(squadsIds);

                if (studentIds == null || studentIds.Count == 0)
                {
                    return NotFound();
                }

                return Ok(studentIds);
            }
            catch
            {
                return StatusCode(500, "Произошла ошибка при попытке получить id студентов");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<int>>> GetStudentsIdsBySquad(int squadsId)
        {
            try
            {
                var studentIds = await _squadStudentService.GetStudentsIdsBySquad(squadsId);

                if (studentIds == null || studentIds.Count == 0)
                {
                    return NotFound();
                }

                return Ok(studentIds);
            }
            catch
            {
                return StatusCode(500, "Произошла ошибка при попытке получить id студентов");
            }
        }
    }
}
