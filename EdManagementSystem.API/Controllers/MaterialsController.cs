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

        [HttpPost]
        public async Task<ActionResult<Material>> CreateMaterial(Material material)
        {
            try
            {
                var createdMaterial = await _materialService.CreateMaterial(material);
                return CreatedAtAction(nameof(GetMaterialById), new { materialId = createdMaterial.MaterialId }, createdMaterial);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
    }
}
