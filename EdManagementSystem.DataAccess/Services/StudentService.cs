using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class StudentService : IStudentService
    {
        private readonly User004Context _context;

        public StudentService(User004Context context)
        {
            _context = context;
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
    }
}
