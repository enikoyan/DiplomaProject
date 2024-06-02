using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ICourseService
    {
        Task<Course> CreateCourse(Course course);
        Task DeleteCourse(int courseId);
        Task DeleteCourse(string courseName);
        Task<List<Course>> GetAllCourses();
        Task<Course> GetCourseById(int courseId);
        Task<Course> GetCourseByName(string courseName);
        Task<int> GetCourseIdByName(string courseName);
        Task<List<Course>> GetCoursesByTutor(int tutorId);
        Task<List<int>> GetCoursesIdsByNames(List<string> courses);
    }
}