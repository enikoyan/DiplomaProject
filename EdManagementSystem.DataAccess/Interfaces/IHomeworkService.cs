using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IHomeworkService
    {
        Task<bool> AddHomeworkFile(Guid homeworkId, List<IFormFile> files);
        Task<bool> CreateHomework(string groupBy, List<string> foreignKeys, string title, string? description, string? note, DateTime? deadline, List<IFormFile>? files);
        Task<bool> DeleteAttachedFile(Guid homeworkId, Guid fileId);
        Task<bool> DeleteHomework(Guid homeworkId, string groupBy, List<string> foreignKeys);
        Task<IActionResult> DownloadHomeworks(Guid homeworkId, string homeworkName);
        Task<List<Homework>> GetAllHomeworks();
        Task<Homework> GetHomeworkById(Guid homeworkId);
        Task<List<Homework>> GetHomeworksByCourse(string courseName);
        Task<List<Homework>> GetHomeworksBySquad(string squadName);
        Task<List<Homework>> GetHomeworksByTitle(string title);
    }
}