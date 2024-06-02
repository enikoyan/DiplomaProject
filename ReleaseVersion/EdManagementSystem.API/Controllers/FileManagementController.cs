using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileManagementController : ControllerBase
    {
        private readonly IFileManagementService _fileManagemenetService;

        public FileManagementController(IFileManagementService fileManagemenetService)
        {
            _fileManagemenetService = fileManagemenetService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string folderName, bool overwrite)
        {
            try
            {
                var filePath = await _fileManagemenetService.UploadFileAsync(file, folderName, overwrite);
                return Ok($"Файл успешно загружен. Путь: {filePath}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFileAsync(string fileName, string folderName)
        {
            try
            {
                return await _fileManagemenetService.DownloadFileAsync(fileName, folderName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{outputFileName}")]
        public async Task<IActionResult> DownloadFileAsync(string fileName, string folderName, string outputFileName)
        {
            try
            {
                return await _fileManagemenetService.DownloadFileAsync(fileName, folderName, outputFileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFileFromDB(Guid fileId, string folderName)
        {
            try
            {
                return await _fileManagemenetService.DownloadFileFromDB(fileId, folderName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFileAsync(string fileName, string folderName)
        {
            try
            {
                await _fileManagemenetService.DeleteFileAsync(fileName, folderName);
                return Ok("Файл успешно удален");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFilesAsync([FromForm] List<string> fileNames, [FromForm] string folderName)
        {
            try
            {
                await _fileManagemenetService.DeleteFileAsync(fileNames, folderName);
                return Ok("Файл успешно удален");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
