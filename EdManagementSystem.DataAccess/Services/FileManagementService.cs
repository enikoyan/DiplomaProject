using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace EdManagementSystem.DataAccess.Services
{
    public class FileManagementService(EdSystemDbContext context) : IFileManagementService
    {
        private const int MaxFileSize = 10 * 1024 * 1024; // 10 MB
        private static readonly string uploadsFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName,
            "EdManagementSystem.DataAccess", "Files");

        private readonly EdSystemDbContext _dbContext = context;

        public async Task<string> UploadFileAsync(IFormFile file, string folderName, bool overwrite = false)
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
                if (overwrite)
                {
                    File.Delete(filePath);
                }
                else throw new ArgumentException("Файл с таким именем уже существует!");
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

        public async Task<FileStreamResult> DownloadFilesAsync(List<string> fileNames, string folderName, List<string> outputFileNames, string archiveName)
        {
            if (fileNames.Count != outputFileNames.Count)
            {
                throw new ArgumentException("Количество входных и выходных имен файлов должно совпадать!");
            }

            string folderPath = Path.Combine(uploadsFolder, folderName);

            byte[] zipBytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < fileNames.Count; i++)
                    {
                        string[] files = Directory.GetFiles(folderPath, fileNames[i] + ".*");

                        if (files.Length > 0)
                        {
                            string filePath = files[0];

                            ZipArchiveEntry entry = zipArchive.CreateEntry(outputFileNames[i] + Path.GetExtension(files[0]));
                            using (Stream entryStream = entry.Open())
                            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                            {
                                await fileStream.CopyToAsync(entryStream);
                            }
                        }
                        else
                        {
                            throw new FileNotFoundException($"Файл {fileNames[i]} не найден в папке {folderPath}!");
                        }
                    }
                }

                zipBytes = memoryStream.ToArray();
            }

            return new FileStreamResult(new MemoryStream(zipBytes), "application/zip")
            {
                FileDownloadName = archiveName + ".zip"
            };
        }

        public async Task<FileStreamResult> DownloadFileFromDB(Guid fileId, string folderName)
        {
            var file = await _dbContext.Files.FirstOrDefaultAsync(s => s.Id == fileId);

            if (file != null)
            {
                return await DownloadFileAsync(fileId.ToString(), folderName, file.Title);
            }
            else throw new Exception("Файл не найден!");
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

        public async Task<bool> DeleteFileAsync(List<string> fileNames, string folderName)
        {
            string folderPath = Path.Combine(uploadsFolder, folderName);

            foreach (var fileName in fileNames)
            {
                string filePath = Directory.GetFiles(folderPath)
                    .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(fileName, StringComparison.OrdinalIgnoreCase));

                if (filePath != null)
                {
                    File.Delete(filePath);
                }

                else
                {
                    throw new FileNotFoundException($"Файл {fileName} не найден!");
                }
            }

            return true;
        }
    }
}
