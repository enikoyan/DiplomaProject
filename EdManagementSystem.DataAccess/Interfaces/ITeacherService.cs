using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ITeacherService
    {
        Task<Teacher> CreateTeacher(Teacher teacher);
        Task DeleteTeacher(int teacherId);
        Task<List<Teacher>> GetAllTeachers();
        Task<List<string>> GetCoursesOfTeacher(string teacherEmail);
        Task<int> GetSquadsCount(string teacherEmail);
        Task<List<string>> GetSquadsOfTeacher(string teacherEmail);
        Task<int> GetStudentsCount(string teacherEmail);
        Task<Teacher> GetTeacherByEmail(string teacherEmail);
    }
}