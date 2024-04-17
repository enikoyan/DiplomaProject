using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class CourseService : ICourseService
    {
        private readonly EdSystemDbContext _context;

        public CourseService(EdSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Course> GetCourseById(int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(u => u.CourseId == courseId);
            if (course == null)
            {
                throw new Exception("Курс не найден!");
            }
            return course;
        }

        public async Task<List<Course>> GetCoursesByTutor(int tutorId)
        {
            var courses = await _context.Courses.Where(u => u.CourseTutor == tutorId).ToListAsync();
            if (courses == null || courses.Count == 0)
            {
                throw new Exception("Курсы не найдены!");
            }
            return courses;
        }

        public async Task<Course> GetCourseByName(string courseName)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(u => u.OptionValue == courseName);
            if (course == null)
            {
                throw new Exception("Курс не найден!");
            }
            return course;
        }

        public async Task<int> GetCourseIdByName(string courseName)
        {
            var courseId = await _context.Courses
                    .Where(u => u.OptionValue == courseName)
                    .Select(u => u.CourseId)
                    .FirstOrDefaultAsync();

            if (courseId == 0)
            {
                throw new Exception("Курс не найден!");
            }
            return courseId;
        }

        public async Task DeleteCourse(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            else throw new Exception("Курс не найден!");
        }

        public async Task DeleteCourse(string courseName)
        {
            var course = await _context.Courses.FindAsync(courseName);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            else throw new Exception("Курс не найден!");
        }

        public async Task<List<Course>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> CreateCourse(Course course)
        {
            var existingUser = await _context.Teachers.FindAsync(course.CourseTutor);

            if (existingUser == null)
            {
                throw new Exception("Преподаватель не найден!");
            }

            // Помечаем сущность как добавляемую в контекст
            _context.Entry(course).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<List<int>> GetCoursesIdsByNames(List<string> courses)
        {
            List<int> coursesIds = new List<int>();

            foreach (var course in courses)
            {
                var courseId = await GetCourseIdByName(course);
                coursesIds.Add(courseId);
            }

            return coursesIds;
        }
    }
}
