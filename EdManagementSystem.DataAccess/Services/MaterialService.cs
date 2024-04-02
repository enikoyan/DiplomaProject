using EdManagementSystem.DataAccess.Data;
using EdManagementSystem.DataAccess.Interfaces;
using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly User004Context _context;

        public MaterialService(User004Context context)
        {
            _context = context;
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
    }
}
