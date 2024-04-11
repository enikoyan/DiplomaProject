using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IFileManagementService
    {
        Task<bool> DeleteFileAsync(string fileName, string folderName);
        Task<FileStreamResult> DownloadFileAsync(string fileName, string folderName);
        Task<FileStreamResult> DownloadFileAsync(string fileName, string folderName, string outputFileName);
        Task<string> UploadFileAsync(IFormFile file, string folderName);
    }
}