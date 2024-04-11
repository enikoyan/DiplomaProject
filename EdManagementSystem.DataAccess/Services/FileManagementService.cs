using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.DataAccess.Services
{
    public class FileManagementService : IFileManagementService
    {
        private const int MaxFileSize = 10 * 1024 * 1024; // 10 MB
        private static readonly string uploadsFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "EdManagementSystem.DataAccess", "Files");

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Файл пустой!");
            }

            if (file.Length > MaxFileSize)
            {
                throw new ArgumentException("Размер файла слишком большой!");
            }

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, folderName, fileName);

            if (!Directory.Exists(Path.Combine(uploadsFolder, folderName)))
            {
                Directory.CreateDirectory(Path.Combine(uploadsFolder, folderName));
            }

            if (File.Exists(filePath))
            {
                throw new ArgumentException("Файл с таким именем уже существует!");
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<FileStreamResult> DownloadFileAsync(string fileName, string folderName)
        {
            string[] allFiles = Directory.GetFiles(Path.Combine(uploadsFolder, folderName));
            string matchingFile = allFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(fileName, StringComparison.OrdinalIgnoreCase));

            if (matchingFile == null)
            {
                throw new FileNotFoundException("Файл не найден!");
            }

            var fileStream = new FileStream(matchingFile, FileMode.Open, FileAccess.Read); // открываем файловый поток

            return new FileStreamResult(fileStream, "application/octet-stream")
            {
                FileDownloadName = Path.GetFileName(matchingFile) // возвращаем найденный файл
            };
        }

        public async Task<FileStreamResult> DownloadFileAsync(string fileName, string folderName, string outputFileName)
        {
            string[] allFiles = Directory.GetFiles(Path.Combine(uploadsFolder, folderName));
            string matchingFile = allFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(fileName, StringComparison.OrdinalIgnoreCase));

            if (matchingFile == null)
            {
                throw new FileNotFoundException("Файл не найден!");
            }

            var fileStream = new FileStream(matchingFile, FileMode.Open, FileAccess.Read); // открываем файловый поток

            return new FileStreamResult(fileStream, "application/octet-stream")
            {
                FileDownloadName = outputFileName + Path.GetExtension(matchingFile)
            };
        }

        public async Task<bool> DeleteFileAsync(string fileName, string folderName)
        {
            string[] allFiles = Directory.GetFiles(Path.Combine(uploadsFolder, folderName));
            string matchingFile = allFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(fileName, StringComparison.OrdinalIgnoreCase));

            if (File.Exists(matchingFile))
            {
                File.Delete(matchingFile);
                return true;
            }

            else
            {
                throw new FileNotFoundException("Файл не найден!");
            }
        }
    }
}
