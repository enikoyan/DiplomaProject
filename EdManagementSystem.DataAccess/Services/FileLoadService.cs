using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EdManagementSystem.DataAccess.Services
{
    public class FileLoadService : IFileLoadService
    {
        private const int MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Файл пустой!");
            }

            if (file.Length > MaxFileSize)
            {
                throw new ArgumentException("Размер файла слишком большой!");
            }

            var uploadsFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "EdManagementSystem.DataAccess", "Materials");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (File.Exists(filePath)) // Проверка на существующий файл
            {
                throw new ArgumentException("Файл с таким именем уже существует!");
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
