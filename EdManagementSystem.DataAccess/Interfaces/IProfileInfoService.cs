using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IProfileInfoService
    {
        Task<List<string>> GetCoursesNamesOfTeacher(string teacherEmail);
        Task<List<string>> GetSquadsNamesOfTeacher(string teacherEmail);
        Task<List<Course>> GetCoursesOfTeacher(string teacherEmail);
        Task<List<Squad>> GetSquadsOfTeacher(string teacherEmail);
        Task<int> GetSquadsCount(string teacherEmail);
        Task<int> GetStudentsCount(string teacherEmail);
    }
}