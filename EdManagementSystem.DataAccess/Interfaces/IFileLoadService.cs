using Microsoft.AspNetCore.Http;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IFileLoadService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}