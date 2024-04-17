using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class StudentService : IStudentService
    {
        private readonly EdSystemDbContext _context;
        private readonly ICourseService _courseService;
        private readonly ISquadService _squadService;
        private readonly ISquadStudentService _squadStudentService;

        public StudentService(EdSystemDbContext context, ICourseService courseService, ISquadService squadService, ISquadStudentService squadStudentService)
        {
            _context = context;
            _courseService = courseService;
            _squadService = squadService;
            _squadStudentService = squadStudentService;
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(u => u.StudentId == studentId);
            if (student == null)
            {
                throw new Exception("Студент не найден!");
            }
            return student;
        }

        public async Task DeleteStudent(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> CreateStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            _context.Students.AddRange(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<List<Student>> GetStudentsByCourse(string courseName)
        {
            List<Student> students = new List<Student>();

            // Get course id by course name 
            var courseId = await _courseService.GetCourseIdByName(courseName);

            // Get squads ids by course id 
            List<int> squadIds = await _squadService.GetSquadsIdsByCourse(courseId);

            // Get students by squad ids 
            var squadStudents = await _squadStudentService.GetStudentsIdsBySquads(squadIds);

            foreach (var studentId in squadStudents)
            {
                var student = await GetStudentById(studentId);
                students.Add(student);
            }

            return students;
        }

        public async Task<List<Student>> GetStudentsBySquad(string squadName)
        {
            List<Student> students = new List<Student>();

            // Get squadId by squadName
            var squad = await _squadService.GetSquadByName(squadName);

            // Get students by squad ids 
            var squadStudents = await _squadStudentService.GetStudentsIdsBySquad(squad.SquadId);

            foreach (var studentId in squadStudents)
            {
                var student = await GetStudentById(studentId);
                students.Add(student);
            }

            return students;
        }
    }
}
