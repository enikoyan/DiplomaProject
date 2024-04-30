using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Services
{
    public interface IHomeworkService
    {
        Task<bool> CreateHomework(string groupBy, List<string> foreignKeys, string title, string description, string? note, DateTime? deadline);
        Task<bool> DeleteHomework(Guid homeworkId);
        Task<List<Homework>> GetAllHomeworks();
        Task<Homework> GetHomeworkById(Guid homeworkId);
        Task<List<Homework>> GetHomeworksByCourse(string courseName);
        Task<List<Homework>> GetHomeworksBySquad(string squadName);
        Task<List<Homework>> GetHomeworksByTitle(string title);
    }
}