using EdManagementSystem.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileLoadController : ControllerBase
    {
        private readonly IFileLoadService _fileLoadService;

        public FileLoadController(IFileLoadService fileLoadService)
        {
            _fileLoadService = fileLoadService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var filePath = await _fileLoadService.UploadFileAsync(file);
                return Ok($"File uploaded successfully. Path: {filePath}");
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
    }
}
