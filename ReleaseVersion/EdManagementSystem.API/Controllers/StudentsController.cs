using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace EdManagementSystem.API.Controllers
{
    [OutputCache]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<Student>> GetStudentById(int studentId)
        {
            var student = await _studentService.GetStudentById(studentId);
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            var createdStudent = await _studentService.CreateStudent(student);
            return CreatedAtAction(nameof(GetStudentById), new { studentId = createdStudent.StudentId }, createdStudent);
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            await _studentService.DeleteStudent(studentId);
            return NoContent();
        }

        [HttpGet]
        [Route("{courseName}")]
        public async Task<IActionResult> GetStudentsByCourse(string courseName)
        {
            try
            {
                var students = await _studentService.GetStudentsByCourse(courseName);

                if (students.Count == 0)
                {
                    return NotFound();
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{squadName}")]
        public async Task<IActionResult> GetStudentsBySquad(string squadName)
        {
            try
            {
                var students = await _studentService.GetStudentsBySquad(squadName);

                if (students.Count == 0)
                {
                    return NotFound();
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
