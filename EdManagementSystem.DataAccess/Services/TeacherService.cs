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

        #region MainInfo
        public async Task<Teacher> GetTeacherByEmail(string teacherEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == teacherEmail);

            if (user == null)
            {
                throw new Exception("Пользователь с таким email не найден!");
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(u => u.TeacherId == user.UserId);

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
        #endregion

        #region StatisticsInfo
        public async Task<int> GetStudentsCount(string teacherEmail)
        {
            // Get teacherId
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserEmail == teacherEmail);
            var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == user.UserId);

            if (teacher == null || user == null)
            {
                throw new Exception("Такого преподавателя нет!");
            }

            // Get courses of this teacher
            List<int> coursesIds = FindCoursesIdsByTutorId(teacher!.TeacherId);

            // Get squads of found courses
            List<int> squadIds = new List<int>();
            foreach (var courseId in coursesIds)
            {
                List<int> squads = FindSquadsIdsByCourseId(courseId);
                squadIds.AddRange(squads);
            }

            // Get count of students of found squads
            return CountStudentsInSquads(squadIds);
        }

        public async Task<int> GetSquadsCount(string teacherEmail)
        {
            // Get teacherId
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserEmail == teacherEmail);
            var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == user.UserId);

            if (teacher == null || user == null)
            {
                throw new Exception("Такого преподавателя нет!");
            }

            // Get courses of this teacher
            List<int> coursesIds = FindCoursesIdsByTutorId(teacher!.TeacherId);

            // Get squads of found courses
            List<int> squadIds = new List<int>();
            foreach (var courseId in coursesIds)
            {
                List<int> squads = FindSquadsIdsByCourseId(courseId);
                squadIds.AddRange(squads);
            };

            // Get count of students of found squads
            return squadIds.Count;
        }

        public async Task<List<string>> GetCoursesOfTeacher(string teacherEmail)
        {
            // Get teacherId
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserEmail == teacherEmail);
            var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == user!.UserId);

            if (teacher == null || user == null)
            {
                throw new Exception("Такого преподавателя нет!");
            }

            // Get courses of this teacher
            return FindCoursesByTutorId(teacher!.TeacherId);
        }

        public async Task<List<string>> GetSquadsOfTeacher(string teacherEmail)
        {
            // Get teacherId
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserEmail == teacherEmail);
            var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == user.UserId);

            if (teacher == null || user == null)
            {
                throw new Exception("Такого преподавателя нет!");
            }

            // Get courses of this teacher
            List<int> coursesIds = FindCoursesIdsByTutorId(teacher!.TeacherId);

            // Get squads of found courses
            List<string> squads = new List<string>();
            foreach (var courseId in coursesIds)
            {
                List<string> squadList = FindSquadsByCourseId(courseId);
                squads.AddRange(squadList);
            };

            return squads;
        }

        #endregion

        #region Private functions
        private List<int> FindCoursesIdsByTutorId(int tutorId)
        {
            List<int> courseIds = _context.Courses
                .Where(c => c.CourseTutor == tutorId)
                .Select(c => c.CourseId)
                .ToList();
            return courseIds;
        }

        private List<int> FindSquadsIdsByCourseId(int courseId)
        {
            List<int> squadIds = new List<int>();
            squadIds = _context.Squads
            .Where(s => s.IdCourse == courseId)
            .Select(s => s.SquadId)
            .ToList();

            return squadIds;
        }

        private int CountStudentsInSquads(List<int> squadIds)
        {
            return _context.SquadStudents.Where(ss => squadIds.Contains(ss.IdSquad)).Select(ss => ss.IdStudent).Distinct().Count();
        }

        private List<string> FindCoursesByTutorId(int tutorId)
        {
            List<string> courses = _context.Courses
                .Where(c => c.CourseTutor == tutorId)
                .Select(c => c.CourseName)
                .ToList();

            return courses;
        }

        private List<string> FindSquadsByCourseId(int courseId)
        {
            List<string> squads = _context.Squads
            .Where(s => s.IdCourse == courseId)
            .Select(s => s.SquadName)
            .ToList();

            return squads;
        }
        #endregion
    }
}
