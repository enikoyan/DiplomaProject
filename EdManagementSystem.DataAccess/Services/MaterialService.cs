using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly User004Context _context;
        private readonly ISquadService _squadService;
        private readonly ICourseService _courseService;

        public MaterialService(User004Context context, ISquadService squadService, ICourseService courseService)
        {
            _context = context;
            _squadService = squadService;
            _courseService = courseService;
        }

        private Guid GenerateGuid() => Guid.NewGuid();

        public async Task<List<Material>> GetAllMaterials()
        {
            return await _context.Materials.ToListAsync();
        }

        public async Task<Material> GetMaterialById(Guid materialId)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.MaterialId == materialId);

            if (material == null)
            {
                throw new Exception("Такого файла нет!");
            }

            else return material;
        }

        public async Task<List<Material>> GetMaterialsByType(string fileType)
        {
            return await _context.Materials.Where(u => u.Type == fileType).ToListAsync();
        }

        public async Task<Material> GetMaterialByTitle(string materialTitle)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Title == materialTitle);

            if (material == null)
            {
                throw new Exception("Такого файла нет!");
            }

            else return material;
        }

        public async Task<Material> CreateMaterial(Material material)
        {
            // Create new Guid for materialId
            material.MaterialId = GenerateGuid();
            material.DateAdded = DateTime.UtcNow;
            _context.Materials.Add(material);

            await _context.SaveChangesAsync();

            return material;
        }

        public async Task<List<Material>> GetMaterialsBySquad(string squadName)
        {
            // Get squadId by squadName
            var squadId = await _squadService.GetSquadIdByName(squadName);

            List<Material> materials = await _context.Materials.Where(m => m.IdSquad == squadId).ToListAsync();

            if (materials == null || materials.Count == 0)
            {
                throw new Exception($"Материалов для группы {squadName} не найдено!");
            }

            return materials;
        }

        public async Task<List<Material>> GetMaterialsByCourse(string courseName)
        {
            // Get courseId by courseName
            var courseId = await _courseService.GetCourseIdByName(courseName);

            List<Material> materials = await _context.Materials.Where(m => m.IdCourse == courseId).ToListAsync();

            if (materials == null || materials.Count == 0)
            {
                throw new Exception($"Материалов для курса {courseName} не найдено!");
            }

            return materials;
        }
    }
}
