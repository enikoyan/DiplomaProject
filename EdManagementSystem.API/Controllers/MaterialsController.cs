using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Material>>> GetAllMaterials()
        {
            var materials = await _materialService.GetAllMaterials();
            if (materials.Count == 0 || materials == null)
            {
                return BadRequest("Материалы не найдены!");
            }
            else return Ok(materials);
        }

        [HttpGet("{materialId}")]
        public async Task<ActionResult<Material>> GetMaterialById(Guid materialId)
        {
            try
            {
                var material = await _materialService.GetMaterialById(materialId);
                return Ok(material);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{fileType}")]
        public async Task<ActionResult<List<Material>>> GetMaterialsByType(string fileType)
        {
            var materials = await _materialService.GetMaterialsByType(fileType);
            return Ok(materials);
        }

        [HttpGet("{materialTitle}")]
        public async Task<ActionResult<Material>> GetMaterialByTitle(string materialTitle)
        {
            try
            {
                var material = await _materialService.GetMaterialByTitle(materialTitle);
                return Ok(material);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{squadName}")]
        public async Task<ActionResult<List<Material>>> GetMaterialsBySquad(string squadName)
        {
            try
            {
                var materials = await _materialService.GetMaterialsBySquad(squadName);
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{courseName}")]
        public async Task<ActionResult<List<Material>>> GetMaterialsByCourse(string courseName)
        {
            try
            {
                var materials = await _materialService.GetMaterialsByCourse(courseName);
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial([FromForm] List<IFormFile> files, [FromForm] string groupBy, [FromForm] List<string> foreignKeys)
        {
            try
            {
                bool result = await _materialService.CreateMaterial(files, groupBy, foreignKeys);
                if (result)
                {
                    return Ok("Материал(-ы) успешно добавлен(-ы)!");
                }
                else
                {
                    return BadRequest("Не удалось загрузить материал(-ы)!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
