using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IHomeworkService
    {
        Task<bool> AddHomeworkFile(Guid homeworkId, List<IFormFile> files);
        Task<bool> ChangeHomeworkDeadline(Guid homeworkId, DateTime deadline);
        Task<bool> CreateHomework(string groupBy, List<string> foreignKeys, string title, string? description, string? note, DateTime? deadline, List<IFormFile>? files);
        Task<bool> DeleteAttachedFile(Guid homeworkId, Guid fileId);
        Task<bool> DeleteHomework(Guid homeworkId, string groupBy, List<string> foreignKeys);
        Task<IActionResult> DownloadHomeworks(Guid homeworkId);
        Task<List<HomeworkDTO>> GetAllHomeworks();
        Task<List<Models.File>> GetAttachedFiles(Guid homeworkId);
        Task<HomeworkDTO> GetHomeworkById(Guid homeworkId);
        Task<List<HomeworkDTO>> GetHomeworksByCourse(string courseName);
        Task<List<HomeworkDTO>> GetHomeworksBySquad(string squadName);
        Task<List<HomeworkDTO>> GetHomeworksByTitle(string title);
        Task<bool> UpdateHomework(Guid homeworkId, string attributeName, string value);
    }
}