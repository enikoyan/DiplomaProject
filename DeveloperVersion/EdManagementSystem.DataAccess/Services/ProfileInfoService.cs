using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class ProfileInfoService : IProfileInfoService
    {
        private readonly EdSystemDbContext _context;

        public ProfileInfoService(EdSystemDbContext context)
        {
            _context = context;
        }

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

        public async Task<List<string>> GetCoursesNamesOfTeacher(string teacherEmail)
        {
            // Get teacherId
            var user = await _context.Users.FirstOrDefaultAsync(s => s.UserEmail == teacherEmail);
            var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == user!.UserId);

            if (teacher == null || user == null)
            {
                throw new Exception("Такого преподавателя нет!");
            }

            // Get courses of this teacher
            return FindCoursesNamesByTutorId(teacher!.TeacherId);
        }

        public async Task<List<string>> GetSquadsNamesOfTeacher(string teacherEmail)
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
                List<string> squadList = FindSquadsNamesByCourseId(courseId);
                squads.AddRange(squadList);
            };

            return squads;
        }

        public async Task<List<Course>> GetCoursesOfTeacher(string teacherEmail)
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

        public async Task<List<Squad>> GetSquadsOfTeacher(string teacherEmail)
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
            List<Squad> squads = new List<Squad>();
            foreach (var courseId in coursesIds)
            {
                List<Squad> squadList = FindSquadsByCourseId(courseId);
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

        private List<string> FindCoursesNamesByTutorId(int tutorId)
        {
            List<string> courses = _context.Courses
                .Where(c => c.CourseTutor == tutorId)
                .Select(c => c.CourseName)
                .ToList();

            return courses;
        }

        private List<string> FindSquadsNamesByCourseId(int courseId)
        {
            List<string> squads = _context.Squads
            .Where(s => s.IdCourse == courseId)
            .Select(s => s.SquadName)
            .ToList();

            return squads;
        }

        private List<Course> FindCoursesByTutorId(int tutorId)
        {
            return _context.Courses.Where(s => s.CourseTutor == tutorId).ToList();
        }

        private List<Squad> FindSquadsByCourseId(int courseId)
        {
            return _context.Squads.Where(s => s.IdCourse == courseId).ToList();
        }
        #endregion
    }
}
