using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IFileManagementService
    {
        Task<bool> DeleteFileAsync(List<string> fileNames, string folderName);
        Task<bool> DeleteFileAsync(string fileName, string folderName);
        Task<FileStreamResult> DownloadFileAsync(string fileName, string folderName);
        Task<FileStreamResult> DownloadFileAsync(string fileName, string folderName, string outputFileName);
        Task<FileStreamResult> DownloadFileFromDB(Guid fileId, string folderName);
        Task<FileStreamResult> DownloadFilesAsync(List<string> fileNames, string folderName, List<string> outputFileNames, string archiveName);
        Task<string> UploadFileAsync(IFormFile file, string folderName, bool overwrite = false);
    }
}