using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceList()
        {
            try
            {
                var attendanceList = await _attendanceService.GetAttendanceList();
                return Ok(attendanceList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendanceItem([FromForm] AttendanceDTO attendance, IFormFile attendanceFile)
        {
            try
            {
                if (attendance == null)
                {
                    return BadRequest("Объект не найден! Попробуйте снова");
                }

                var success = await _attendanceService.CreateAttendanceItem(attendance, attendanceFile);

                if (success)
                {
                    return Ok("Посещаемость успешно загружена на сервер!");
                }
                else
                {
                    return BadRequest("Не удалось загрузить посещаемость на сервер");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("{attendanceId}")]
        public async Task<IActionResult> RefreshAttendanceFile(Guid attendanceId, IFormFile file)
        {
            try
            {
                bool result = await _attendanceService.RefreshAttendanceFile(attendanceId, file, true);

                if (result)
                {
                    return Ok("Посещаемость успешно обновлена!");
                }
                else
                {
                    return BadRequest("Не удалось обновить посещаемость!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Возникла ошибка: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendanceMatrix([FromForm] AttendanceDTO attendanceDTO)
        {
            var matrix = await _attendanceService.CreateAttendanceMatrix(attendanceDTO);
            if (matrix != null)
            {
                return Ok(matrix);
            }
            return NotFound();
        }
    }
}
