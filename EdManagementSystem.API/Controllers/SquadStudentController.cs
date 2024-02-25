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

    }
}
