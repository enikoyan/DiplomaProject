﻿using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeachers();
            if (teachers == null)
            {
                return NotFound();
            }

            return Ok(teachers);
        }

        [HttpGet("{teacherEmail}")]
        public async Task<IActionResult> GetTeacherByEmail(string teacherEmail)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherByEmail(teacherEmail);
                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] Teacher teacher)
        {
            try
            {
                var createdTeacher = await _teacherService.CreateTeacher(teacher);
                return Ok(createdTeacher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{teacherId}")]
        public async Task<ActionResult> DeleteTeacher(int teacherId)
        {
            try
            {
                await _teacherService.DeleteTeacher(teacherId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
