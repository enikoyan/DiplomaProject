using EdManagementSystem.DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace EdManagementSystem.DataAccess.Interfaces
{
    public interface IMaterialService
    {
        Task<List<Material>> GetAllMaterials();
        Task<Material> GetMaterialById(Guid materialId);
        Task<Material> GetMaterialByTitle(string materialTitle);
        Task<List<Material>> GetMaterialsByType(string fileType);
        Task<List<Material>> GetMaterialsBySquad(string squadName);
        Task<List<Material>> GetMaterialsByCourse(string courseName);
        Task<bool> CreateMaterial(List<IFormFile> files, string groupBy, List<string> foreignKeys);
    }
}