using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SchedulesController(IScheduleService scheduleService) : ControllerBase
    {
        private readonly IScheduleService _scheduleService = scheduleService;

        [HttpGet]
        public async Task<ActionResult<List<Schedule>>> GetAllScheduleItems()
        {
            try
            {
                var schedules = await _scheduleService.GetAllScheduleItems();
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Schedule>>> GetScheduleByWeek(string teacherEmail, DateOnly weekStart, DateOnly weekEnd)
        {
            try
            {
                var schedules = await _scheduleService.GetScheduleByWeek(teacherEmail, weekStart, weekEnd);
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Schedule>>> GetScheduleBySquad(string squadName)
        {
            try
            {
                var schedules = await _scheduleService.GetScheduleBySquad(squadName);
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Schedule>>> GetScheduleByDay(DateOnly date)
        {
            try
            {
                var schedules = await _scheduleService.GetScheduleByDay(date);
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateSchedule(List<Schedule> schedules)
        {
            try
            {
                var success = await _scheduleService.CreateSchedule(schedules);
                if (success)
                    return NoContent();
                else
                    return BadRequest("Не удалось создать расписание!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}