using EdManagementSystem.DataAccess.Models;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IMaterialService
    {
        Task<Material> CreateMaterial(Material material);
        Task<List<Material>> GetAllMaterials();
        Task<Material> GetMaterialById(Guid materialId);
        Task<Material> GetMaterialByTitle(string materialTitle);
        Task<List<Material>> GetMaterialsByType(string fileType);
    }
}