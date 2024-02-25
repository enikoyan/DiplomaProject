using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly User004Context _context;

        public TeacherService(User004Context context)
        {
            _context = context;
        }

        public async Task<Teacher> GetTeacherById(int teacherId)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(u => u.TeacherId == teacherId);
            if (teacher == null)
            {
                throw new Exception("Преподаватель не найден!");
            }
            return teacher;
        }
        public async Task DeleteTeacher(int teacherId)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Teacher>> GetAllTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher> CreateTeacher(Teacher teacher)
        {
            var existingUser = await _context.Users.FindAsync(teacher.TeacherId);

            if (existingUser == null)
            {
                throw new Exception("Пользователь не найден!");
            }

            // Помечаем сущность как добавляемую в контекст
            _context.Entry(teacher).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return teacher;
        }
    }
}
