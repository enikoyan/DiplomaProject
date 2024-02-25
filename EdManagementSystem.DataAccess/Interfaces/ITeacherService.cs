using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface ITeacherService
    {
        Task<Teacher> CreateTeacher(Teacher teacher);
        Task DeleteTeacher(int teacherId);
        Task<List<Teacher>> GetAllTeachers();
        Task<Teacher> GetTeacherById(int teacherId);
    }
}