namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IProfileInfoService
    {
        Task<List<string>> GetCoursesOfTeacher(string teacherEmail);
        Task<int> GetSquadsCount(string teacherEmail);
        Task<List<string>> GetSquadsOfTeacher(string teacherEmail);
        Task<int> GetStudentsCount(string teacherEmail);
    }
}